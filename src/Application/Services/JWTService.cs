using Application.Interfaces;
using Shared.Types;
using Application.UseCases.JWT;
using Domain.Entities.SingleIdEntities;

namespace Application.Services;

public class JwtService(GenerateTokenPairUseCase generateTokenPairUseCase, RenewTokenPairUseCase renewTokenPairUseCase) : IJWTService
{
    private readonly GenerateTokenPairUseCase _generateTokenPairUseCase = generateTokenPairUseCase;
    private readonly RenewTokenPairUseCase _renewTokenPairUseCase = renewTokenPairUseCase;

    public Task<Result<JWTPairResponse>> GenerateTokenPair(User user)
    {
        return _generateTokenPairUseCase.Execute(user);
    }

    public Task<Result<JWTPairResponse>> RenewTokenPair(string refreshToken)
    {
        return _renewTokenPairUseCase.Execute(refreshToken);
    }
}
