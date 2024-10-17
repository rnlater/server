using Application.DTOs;
using Domain.Base;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Config;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.JWT;

public class GenerateTokenPairUseCase : AccessTokenGenerator, IUseCase<(string, string), UserDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GenerateTokenPairUseCase(IOptions<JwtSettings> jwtOptions, IUnitOfWork unitOfWork) : base(jwtOptions)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<(string, string)>> Execute(UserDto parameters)
    {
        var tokenRepository = _unitOfWork.Repository<Authentication>();

        var userAuthentication = await tokenRepository
            .Find(new BaseSpecification<Authentication>(rt => rt.UserId == parameters.Id));
        if (userAuthentication == null)
        {
            return Result<(string, string)>.Fail(ErrorMessage.UserNotFound);
        }

        string refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        userAuthentication.RefreshToken = refreshToken;
        userAuthentication.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await tokenRepository.Update(userAuthentication);

        await _unitOfWork.SaveChangesAsync();

        string accessToken = AccessToken(parameters);

        return Result<(string, string)>.Done((accessToken, refreshToken));
    }
}
