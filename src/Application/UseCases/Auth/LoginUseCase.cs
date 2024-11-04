using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Auth;

public class LoginParams
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginUseCase : IUseCase<UserDto, LoginParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LoginUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Execute(LoginParams parameters)
    {
        try
        {
            var userRepository = _unitOfWork.Repository<User>();
            var user = await userRepository.Find(
                new BaseSpecification<User>(u => u.Email == parameters.Email)
                .AddInclude(query => query.Include(u => u.Authentication!)));

            if (user == null)
                return Result<UserDto>.Fail(ErrorMessage.UserNotFoundWithEmail);
            else if (!user.Authentication!.VerifyPassword(parameters.Password))
                return Result<UserDto>.Fail(ErrorMessage.WrongPassword);
            else if (!user.Authentication!.IsEmailConfirmed)
                return Result<UserDto>.Fail(ErrorMessage.EmailNotConfirmed);
            else if (!user.Authentication!.IsActivated)
                return Result<UserDto>.Fail(ErrorMessage.AccountIsLocked);

            return Result<UserDto>.Done(_mapper.Map<UserDto>(user));
        }
        catch (Exception)
        {
            return Result<UserDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}
