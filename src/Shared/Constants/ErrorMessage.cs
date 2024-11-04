namespace Shared.Constants;

public enum ErrorMessage
{
    UnknownError,

    #region Authentication

    WrongPassword,
    EmailNotConfirmed,
    AccountIsLocked,
    InvalidConfirmationCode,
    ConfirmationCodeExpired,
    EmailAlreadyConfirmed,
    UserAlreadyLoggedOut,

    #endregion

    #region JWT

    AccessTokenIsInvalid,
    AccessTokenIsExpired,
    RefreshTokenNotFound,
    RefreshTokenIsInvalid,
    RefreshTokenIsExpired,

    #endregion

    #region User

    UserNotFound,
    UserNotFoundWithEmail,
    UserAlreadyExists,
    UserAlreadyExistsWithSameEmail,
    UserIsNotActive,

    #endregion

    #region Authorization

    UserIsNotAdmin,

    #endregion

    #region Track

    NoTrackFound,
    NoTrackFoundWithSearch,
    NoTrackFoundWithGuid,

    #endregion

    #region Subject

    NoSubjectFound,
    NoSubjectFoundWithGuid

    #endregion
}
