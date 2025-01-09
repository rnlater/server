using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;
using Shared.Utils;

namespace Application.UseCases.Auth;

public class RegisterParams
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class RegisterUseCase : IUseCase<UserDto, RegisterParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMailService _mailService;

    public RegisterUseCase(IUnitOfWork unitOfWork, IMapper mapper, IMailService mailService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mailService = mailService;
    }

    public async Task<Result<UserDto>> Execute(RegisterParams parameters)
    {
        try
        {
            var userRepository = _unitOfWork.Repository<User>();
            var user = await userRepository.Find(
                new BaseSpecification<User>(x => x.Email == parameters.Email)
                .AddInclude(query => query.Include(x => x.Authentication!))
            );
            if (user != null)
            {
                if (user.Authentication != null && !user.Authentication.IsEmailConfirmed)
                    return Result<UserDto>.Fail(ErrorMessage.EmailNotConfirmed);
                return Result<UserDto>.Fail(ErrorMessage.UserAlreadyExistsWithSameEmail);
            };
            var savedUser = await userRepository.Add(new User
            {
                Email = parameters.Email,
                UserName = "User-" + Guid.NewGuid().ToString().Substring(0, 8),
            });

            var userAuthenticationRepository = _unitOfWork.Repository<Authentication>();
            var ConfirmationCode = Guid.NewGuid().ToString()[..6];
            var authentication = new Authentication
            {
                UserId = savedUser.Id,
                HashedPassword = PasswordHasher.HashWithSHA256(parameters.Password),
                ConfirmationCode = ConfirmationCode,
                ConfirmationCodeExpiryTime = DateTime.UtcNow.AddMinutes(15),
                IsEmailConfirmed = false,
                IsActivated = true,
            };
            await userAuthenticationRepository.Add(authentication);
            await _mailService.SendEmail(
                savedUser.Email,
                savedUser.UserName,
                "Email Confirmation",
                $"Your confirmation code is: {ConfirmationCode}"
            );

            var userDto = _mapper.Map<UserDto>(savedUser);
            userDto.ConfirmationCodeExpiryTime = authentication.ConfirmationCodeExpiryTime;

            return Result<UserDto>.Done(userDto);
        }
        catch (Exception)
        {
            return Result<UserDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}
