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

    #region Track

    public const string GetTracks = "list";
    public const string GetDetailedTracks = "list-detailed";
    public const string GetTrackById = "detailed/{id}";
    public const string CreateTrack = "create";
    public const string UpdateTrack = "update";
    public const string DeleteTrack = "delete/{id}";
    public const string CreateDeleteTrackSubject = "add-remove-subject";

    #endregion
}
