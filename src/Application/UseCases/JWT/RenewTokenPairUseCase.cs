using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.JWT;

public class RenewTokenPairUseCase : IUseCase<JWTPairResponse, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GenerateTokenPairUseCase _generateTokenPairUseCase;
    public RenewTokenPairUseCase(IUnitOfWork unitOfWork, GenerateTokenPairUseCase generateTokenPairUseCase)
    {
        _unitOfWork = unitOfWork;
        _generateTokenPairUseCase = generateTokenPairUseCase;
    }

    public async Task<Result<JWTPairResponse>> Execute(string refreshToken)
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
                return Result<JWTPairResponse>.Fail(ErrorMessage.UserNotFound);
            }
            else if (authentication.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return Result<JWTPairResponse>.Fail(ErrorMessage.RefreshTokenIsExpired);
            }

            Result<JWTPairResponse> tokenPairResult = await _generateTokenPairUseCase.Execute(user);
            return tokenPairResult;
        }
        catch
        {
            return Result<JWTPairResponse>.Fail(ErrorMessage.UnknownError);
        }
    }


}
