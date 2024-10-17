using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Auth;

public class ConfirmPasswordResettingEmailParams
{
    public required string Email { get; set; }
    public required string ConfirmationCode { get; set; }
    public required string Password { get; set; }
}
public class ConfirmPasswordResettingEmailUseCase : IUseCase<UserDto, ConfirmPasswordResettingEmailParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ConfirmPasswordResettingEmailUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Execute(ConfirmPasswordResettingEmailParams parameters)
    {
        try
        {
            var userRepository = _unitOfWork.Repository<User>();
            var user = await userRepository.Find(new BaseSpecification<User>(x => x.Email == parameters.Email).AddInclude(x => x.Authentication!));
            if (user == null || user.Authentication == null)
                return Result<UserDto>.Fail(ErrorMessage.UserNotFoundWithEmail);
            else if (user.Authentication.ConfirmationCode == null || user.Authentication.ConfirmationCodeExpiryTime == null)
                return Result<UserDto>.Fail(ErrorMessage.EmailAlreadyConfirmed);
            else if (user.Authentication.ConfirmationCode != parameters.ConfirmationCode)
                return Result<UserDto>.Fail(ErrorMessage.InvalidConfirmationCode);
            else if (user.Authentication.ConfirmationCodeExpiryTime < DateTime.UtcNow)
                return Result<UserDto>.Fail(ErrorMessage.ConfirmationCodeExpired);
            else if (!user.Authentication.IsActivated)
                return Result<UserDto>.Fail(ErrorMessage.AccountIsLocked);

            user.Authentication.SetPassword(parameters.Password);
            user.Authentication.ConfirmationCode = null;
            user.Authentication.ConfirmationCodeExpiryTime = null;
            await userRepository.Update(user);

            return Result<UserDto>.Done(_mapper.Map<UserDto>(user));
        }
        catch (Exception)
        {
            return Result<UserDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}
