using Application.UseCases.JWT;
using Domain.Entities.SingleIdEntities;
using Shared.Types;

namespace Application.Interfaces;

public interface IJWTService
{
    /// <summary>
    /// Generates a pair of access token and refresh token.
    /// </summary>
    /// <param name="userId">The user's GUID.</param>
    /// <returns>A `Task` that returns a `Result` containing a `JWTPairResponse`.</returns>
    /// <exception cref="ErrorMessage.UserNotFound">Thrown when the user or their authentication does not exist.</exception>
    Task<Result<JWTPairResponse>> GenerateTokenPair(User user);

    /// <summary>
    /// Renews the access token using the refresh token.
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns>A `Task` that returns a `Result` containing a `JWTPairResponse`.</returns>
    /// <exception cref="ErrorMessage.UserNotFound">Thrown when the user or their authentication isn't found with the refresh token.</exception>
    /// <exception cref="ErrorMessage.RefreshTokenIsExpired">Thrown when the refresh token has expired.</exception>
    Task<Result<JWTPairResponse>> RenewTokenPair(string refreshToken);
}
