using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Config;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.JWT;

public class RenewAccessTokenUseCase : AccessTokenGenerator, IUseCase<string, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper mapper;
    public RenewAccessTokenUseCase(IUnitOfWork unitOfWork, IMapper mapper, IOptions<JwtSettings> jwtOptions) : base(jwtOptions)
    {
        _unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<Result<string>> Execute(string refreshToken)
    {

        var authentication = await _unitOfWork
            .Repository<Authentication>()
            .Find(new BaseSpecification<Authentication>(rt => rt.RefreshToken == refreshToken).AddInclude(rt => rt.User!));

        var user = authentication?.User;

        if (authentication == null || user == null)
        {
            return Result<string>.Fail(ErrorMessage.UserNotFound);
        }
        else if (authentication.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            return Result<string>.Fail(ErrorMessage.RefreshTokenIsExpired);
        }

        string accessToken = AccessToken(mapper.Map<UserDto>(user)!);

        return Result<string>.Done(accessToken);
    }
}
