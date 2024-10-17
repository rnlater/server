using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Auth;

public class ForgotPasswordParams
{
    public required string Email { get; set; }
}
public class ForgotPasswordUseCase : IUseCase<UserDto, ForgotPasswordParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ForgotPasswordUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Execute(ForgotPasswordParams parameters)
    {
        try
        {
            var userRepository = _unitOfWork.Repository<User>();
            var user = await userRepository.Find(new BaseSpecification<User>(x => x.Email == parameters.Email).AddInclude(x => x.Authentication!));
            if (user == null || user.Authentication == null)
                return Result<UserDto>.Fail(ErrorMessage.UserNotFoundWithEmail);
            else if (!user.Authentication.IsEmailConfirmed)
                return Result<UserDto>.Fail(ErrorMessage.EmailNotConfirmed);
            else if (!user.Authentication.IsActivated)
                return Result<UserDto>.Fail(ErrorMessage.AccountIsLocked);

            user.Authentication.ConfirmationCode = Guid.NewGuid().ToString()[..6];
            user.Authentication.ConfirmationCodeExpiryTime = DateTime.UtcNow.AddMinutes(15);
            await userRepository.Update(user);

            return Result<UserDto>.Done(_mapper.Map<UserDto>(user));
        }
        catch (Exception)
        {
            return Result<UserDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}
