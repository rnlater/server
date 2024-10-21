using Application.DTOs;
using Application.UseCases.Auth;
using Shared.Types;

namespace Application.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Login user
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of the logged in user</returns>
    Task<Result<UserDto>> Login(LoginParams Params);

    /// <summary>
    /// Register user
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of the registered user</returns>
    Task<Result<UserDto>> Register(RegisterParams Params);

    /// <summary>
    /// Logout user
    /// </summary>
    /// <returns>return result of the logged out user</returns>
    Task<Result<UserDto>> Logout();

    /// <summary>
    /// Confirm registration email
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of the registered user</returns>
    Task<Result<UserDto>> ConfirmRegistrationEmail(ConfirmRegistrationEmailParams Params);

    /// <summary>
    /// Send email to reset password
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of the user that forgot his/her password</returns>
    Task<Result<UserDto>> ForgotPassword(ForgotPasswordParams Params);

    /// <summary>
    /// Confirm password resetting email
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of the user that forgot his/her password</returns>
    Task<Result<UserDto>> ConfirmPasswordResettingEmail(ConfirmPasswordResettingEmailParams Params);
}

