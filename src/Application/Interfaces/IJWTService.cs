using Application.DTOs;
using Shared.Types;

namespace Application.Interfaces;

public interface IJWTService
{
    /// <summary>
    /// Generates a pair of JWT tokens for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<Result<(string, string)>> GenerateTokenPair(UserDto user);

    /// <summary>
    /// Renews the access token using the refresh token.
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    Task<Result<string>> RenewAccessToken(string refreshToken);
}
