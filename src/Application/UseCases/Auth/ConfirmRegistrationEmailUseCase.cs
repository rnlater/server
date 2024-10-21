using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Auth;

public class ConfirmRegistrationEmailParams
{
    public required string Email { get; set; }
    public required string ConfirmationCode { get; set; }
}
public class ConfirmRegistrationEmailUseCase : IUseCase<UserDto, ConfirmRegistrationEmailParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ConfirmRegistrationEmailUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Execute(ConfirmRegistrationEmailParams parameters)
    {
        try
        {
            var userRepositoy = _unitOfWork.Repository<User>();
            var user = await userRepositoy.Find(new BaseSpecification<User>(x => x.Email == parameters.Email).AddInclude(x => x.Authentication!));
            if (user == null || user.Authentication == null)
                return Result<UserDto>.Fail(ErrorMessage.UserNotFoundWithEmail);
            else if (user.Authentication.ConfirmationCode == null || user.Authentication.IsEmailConfirmed || user.Authentication.ConfirmationCodeExpiryTime == null)
                return Result<UserDto>.Fail(ErrorMessage.EmailAlreadyConfirmed);
            else if (user.Authentication.ConfirmationCode != parameters.ConfirmationCode)
                return Result<UserDto>.Fail(ErrorMessage.InvalidConfirmationCode);
            else if (user.Authentication.ConfirmationCodeExpiryTime < DateTime.UtcNow)
                return Result<UserDto>.Fail(ErrorMessage.ConfirmationCodeExpired);

            user.Authentication.IsEmailConfirmed = true;
            user.Authentication.ConfirmationCode = null;
            user.Authentication.ConfirmationCodeExpiryTime = null;
            await userRepositoy.Update(user);

            return Result<UserDto>.Done(_mapper.Map<UserDto>(user));
        }
        catch (Exception)
        {
            return Result<UserDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}
