using System.Security.Claims;
using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Auth;

public class LogoutUseCase : IUseCase<UserDto, NoParam>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public LogoutUseCase(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Execute(NoParam none)
    {
        try
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userAuthenticationRepository = _unitOfWork.Repository<Authentication>();
            var userAuthentication = await userAuthenticationRepository.Find(new BaseSpecification<Authentication>(x => x.UserId == Guid.Parse(userId!)).AddInclude(x => x.User!));

            if (userAuthentication == null)
                return Result<UserDto>.Fail(ErrorMessage.UserNotFound);
            else if (userAuthentication.RefreshToken == null || userAuthentication.RefreshTokenExpiryTime == null)
                return Result<UserDto>.Fail(ErrorMessage.UserAlreadyLoggedOut);

            userAuthentication!.RefreshToken = null;
            userAuthentication.RefreshTokenExpiryTime = null;
            await userAuthenticationRepository.Update(userAuthentication);

            var userDto = _mapper.Map<UserDto>(userAuthentication.User!);
            return Result<UserDto>.Done(userDto);
        }
        catch (Exception)
        {
            return Result<UserDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}
