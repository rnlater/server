using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Auth;

public class ResendCodeParams
{
    public required string Email { get; set; }
}
public class ResendCodeUseCase : IUseCase<UserDto, ResendCodeParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMailService _mailService;

    public ResendCodeUseCase(IUnitOfWork unitOfWork, IMapper mapper, IMailService mailService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mailService = mailService;
    }

    public async Task<Result<UserDto>> Execute(ResendCodeParams parameters)
    {
        try
        {
            var userRepository = _unitOfWork.Repository<User>();
            var authenticationRepository = _unitOfWork.Repository<Authentication>();

            var user = await userRepository.Find(
                new BaseSpecification<User>(u => u.Email == parameters.Email)
                .AddInclude(query => query.Include(u => u.Authentication!)));

            if (user == null || user.Authentication == null)
                return Result<UserDto>.Fail(ErrorMessage.UserNotFoundWithEmail);
            else if (user.Authentication.ConfirmationCodeExpiryTime > DateTime.UtcNow)
                return Result<UserDto>.Fail(ErrorMessage.ConfirmationCodeNotExpired);
            else if (!user.Authentication.IsActivated)
                return Result<UserDto>.Fail(ErrorMessage.AccountIsLocked);

            var ConfirmationCode = Guid.NewGuid().ToString()[..6];
            var authentication = user.Authentication;
            authentication.User = null;
            authentication.ConfirmationCode = ConfirmationCode;
            authentication.ConfirmationCodeExpiryTime = DateTime.UtcNow.AddMinutes(15);
            await authenticationRepository.Update(authentication);

            user.Authentication = null;
            await _mailService.SendEmail(
                user.Email,
                user.UserName,
                "Resend Confirmation Code",
                $"Your confirmation code is {ConfirmationCode}"
            );

            var userDto = _mapper.Map<UserDto>(user);
            userDto.ConfirmationCodeExpiryTime = authentication.ConfirmationCodeExpiryTime;

            return Result<UserDto>.Done(userDto);
        }
        catch (Exception)
        {
            return Result<UserDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}
