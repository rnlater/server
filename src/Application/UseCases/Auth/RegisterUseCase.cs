using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities;
using Domain.Interfaces;
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

    public RegisterUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Execute(RegisterParams parameters)
    {
        try
        {
            var userRepository = _unitOfWork.Repository<User>();
            var user = await userRepository.Find(
                new BaseSpecification<User>(x => x.Email == parameters.Email)
            );
            if (user != null) return Result<UserDto>.Fail(ErrorMessage.UserAlreadyExistsWithSameEmail);
            var result = await userRepository.Add(new User
            {
                Email = parameters.Email,
                UserName = "User-" + Guid.NewGuid().ToString().Substring(0, 8),
            });

            var userAuthenticationRepository = _unitOfWork.Repository<Authentication>();
            await userAuthenticationRepository.Add(new Authentication
            {
                UserId = result.Id,
                HashedPassword = PasswordHasher.HashWithSHA256(parameters.Password),
                ConfirmationCode = Guid.NewGuid().ToString()[..6],
                ConfirmationCodeExpiryTime = DateTime.UtcNow.AddMinutes(15),
                IsEmailConfirmed = false,
                IsActivated = true,
            });

            return Result<UserDto>.Done(_mapper.Map<UserDto>(result));
        }
        catch (Exception)
        {
            return Result<UserDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}
