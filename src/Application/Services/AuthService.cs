using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Auth;
using Domain.Interfaces;
using Shared.Types;

namespace Application.Services;

public class AuthService(
    LoginUseCase loginUseCase,
    RegisterUseCase registerUseCase,
    ForgotPasswordUseCase forgotPasswordUseCase,
    ConfirmRegistrationEmailUseCase confirmRegistrationEmailUseCase,
    ConfirmPasswordResettingEmailUseCase confirmPasswordResettingEmailUseCase,
    LogoutUseCase logoutUseCase) : IAuthService
{
    private readonly LoginUseCase loginUseCase = loginUseCase;
    private readonly RegisterUseCase registerUseCase = registerUseCase;
    private readonly ForgotPasswordUseCase forgotPasswordUseCase = forgotPasswordUseCase;
    private readonly ConfirmRegistrationEmailUseCase confirmRegistrationEmailUseCase = confirmRegistrationEmailUseCase;
    private readonly ConfirmPasswordResettingEmailUseCase confirmPasswordResettingEmailUseCase = confirmPasswordResettingEmailUseCase;
    private readonly LogoutUseCase logoutUseCase = logoutUseCase;

    public Task<Result<UserDto>> Login(LoginParams Params)
    {
        return loginUseCase.Execute(Params);

    }
    public Task<Result<UserDto>> Register(RegisterParams Params)
    {

        return registerUseCase.Execute(Params);
    }

    public Task<Result<UserDto>> ConfirmRegistrationEmail(ConfirmRegistrationEmailParams Params)
    {
        return confirmRegistrationEmailUseCase.Execute(Params);
    }

    public Task<Result<UserDto>> ForgotPassword(ForgotPasswordParams Params)
    {
        return forgotPasswordUseCase.Execute(Params);
    }

    public Task<Result<UserDto>> ConfirmPasswordResettingEmail(ConfirmPasswordResettingEmailParams Params)
    {
        return confirmPasswordResettingEmailUseCase.Execute(Params);
    }

    public Task<Result<UserDto>> Logout()
    {
        return logoutUseCase.Execute(NoParam.Value);
    }
}
