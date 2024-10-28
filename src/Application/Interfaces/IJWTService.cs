using Application.DTOs;
using Shared.Types;

namespace Application.Interfaces;

public interface IJWTService
{
    /// <summary>
    /// Generates a pair of access token and refresh token.
    /// </summary>
    /// <param name="user">The user data transfer object containing user information.</param>
    /// <returns>A `Task` that returns a `Result` containing a tuple with the access token and refresh token.</returns>
    /// <exception cref="ErrorMessage.UserNotFound">Thrown when the user or their authentication does not exist.</exception>
    Task<Result<(string, string)>> GenerateTokenPair(UserDto user);

    /// <summary>
    /// Renews the access token using the refresh token.
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    /// <exception cref="ErrorMessage.UserNotFound">Thrown when the user or their authentication isn't found with the refresh token.</exception>
    /// <exception cref="ErrorMessage.RefreshTokenIsExpired">Thrown when the refresh token has expired.</exception>
    Task<Result<string>> RenewAccessToken(string refreshToken);
}
