using Application.Interfaces;
using Domain.Entities;
using Shared.Types;
using Application.UseCases.JWT;
using Application.DTOs;

namespace Application.Services;

public class JwtService(GenerateTokenPairUseCase generateTokenPairUseCase, RenewAccessTokenUseCase renewAccessTokenUseCase) : IJWTService
{
    private readonly GenerateTokenPairUseCase _generateTokenPairUseCase = generateTokenPairUseCase;
    private readonly RenewAccessTokenUseCase _renewAccessTokenUseCase = renewAccessTokenUseCase;

    public Task<Result<(string, string)>> GenerateTokenPair(UserDto user)
    {
        return _generateTokenPairUseCase.Execute(user);
    }

    public Task<Result<string>> RenewAccessToken(string refreshToken)
    {
        return _renewAccessTokenUseCase.Execute(refreshToken);
    }
}
