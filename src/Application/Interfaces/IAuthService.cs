using Application.DTOs;
using Application.UseCases.Auth;
using Application.UseCases.JWT;
using Shared.Types;

namespace Application.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Login user
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of the logged in user</returns>
    /// <exception cref="UserNotFoundWithEmail">Thrown when the user is not found with the email</exception>
    /// <exception cref="WrongPassword">Thrown when the password is wrong</exception>
    /// <exception cref="EmailNotConfirmed">Thrown when the email is not confirmed</exception>
    /// <exception cref="AccountIsLocked">Thrown when the account is locked</exception>
    Task<Result<(UserDto, JWTPairResponse)>> Login(LoginParams Params);

    /// <summary>
    /// Register user
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of the registered user</returns>
    /// <exception cref="UserAlreadyExistsWithSameEmail">Thrown when the user already exists with the same email</exception>
    Task<Result<UserDto>> Register(RegisterParams Params);

    /// <summary>
    /// Logout user
    /// </summary>
    /// <returns>return result of the logged out user</returns>
    /// <exception cref="UserNotFound">Thrown when the user is not found</exception>
    /// <exception cref="UserAlreadyLoggedOut">Thrown when the user is already logged out</exception>
    Task<Result<UserDto>> Logout();

    /// <summary>
    /// Confirm registration email
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of the registered user</returns>
    /// <exception cref="UserNotFoundWithEmail">Thrown when the user is not found with the email</exception>
    /// <exception cref="EmailAlreadyConfirmed">Thrown when the email is already confirmed</exception>
    /// <exception cref="InvalidConfirmationCode">Thrown when the confirmation code is invalid</exception>
    /// <exception cref="ConfirmationCodeExpired">Thrown when the confirmation code is expired</exception>
    Task<Result<(UserDto, JWTPairResponse)>> ConfirmRegistrationEmail(ConfirmRegistrationEmailParams Params);

    /// <summary>
    /// Send email to reset password
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of the user that forgot his/her password</returns>
    /// <exception cref="UserNotFoundWithEmail">Thrown when the user is not found with the email</exception>
    /// <exception cref="EmailNotConfirmed">Thrown when the email is not confirmed</exception>
    /// <exception cref="AccountIsLocked">Thrown when the account is locked</exception>
    Task<Result<UserDto>> ForgotPassword(ForgotPasswordParams Params);

    /// <summary>
    /// Resend confirmation code
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of the user that forgot his/her password</returns>
    /// <exception cref="UserNotFoundWithEmail">Thrown when the user is not found with the email</exception>
    /// <exception cref="ConfirmationCodeNotExpired">Thrown when the confirmation code is not expired</exception>
    /// <exception cref="AccountIsLocked">Thrown when the account is locked</exception>
    Task<Result<UserDto>> ResendCode(ResendCodeParams Params);



    /// <summary>
    /// Confirm password resetting email
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of the user that forgot his/her password</returns>
    /// <exception cref="UserNotFoundWithEmail">Thrown when the user is not found with the email</exception>
    /// <exception cref="EmailAlreadyConfirmed">Thrown when the email is already confirmed</exception>
    /// <exception cref="InvalidConfirmationCode">Thrown when the confirmation code is invalid</exception>
    /// <exception cref="ConfirmationCodeExpired">Thrown when the confirmation code is expired</exception>
    /// <exception cref="AccountIsLocked">Thrown when the account is locked</exception>
    Task<Result<(UserDto, JWTPairResponse)>> ConfirmPasswordResettingEmail(ConfirmPasswordResettingEmailParams Params);
}

