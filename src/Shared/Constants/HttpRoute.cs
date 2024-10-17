namespace Shared.Constants;

public class HttpRoute
{
    #region JWT

    public const string RefreshAccessToken = "refresh-access-token";

    #endregion

    #region Auth

    public const string Login = "login";
    public const string Register = "register";
    public const string ConfirmRegistrationEmail = "confirm-registration-email";
    public const string ForgotPassword = "forgot-password";
    public const string ConfirmPasswordResettingEmail = "confirm-password-resetting-email";
    public const string Logout = "logout";

    #endregion
}
