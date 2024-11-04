using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        try
        {
            var authentication = await _unitOfWork
                .Repository<Authentication>().Find(
                    new BaseSpecification<Authentication>(a => a.RefreshToken == refreshToken)
                    .AddInclude(query => query.Include(a => a.User!)));

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
        catch
        {
            return Result<string>.Fail(ErrorMessage.UnknownError);
        }
    }
}
