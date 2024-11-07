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

    #region Subject

    public const string GetSubjects = "list";
    public const string GetSubjectById = "detailed/{id}";
    public const string CreateSubject = "create";
    public const string UpdateSubject = "update";
    public const string DeleteSubject = "delete/{id}";
    public const string CreateDeleteSubjectKnowledge = "add-remove-knowledge";

    #endregion

    #region KnowledgeType

    public const string GetKnowledgeTypes = "list";
    public const string GetKnowledgeTypeByGuid = "detailed/{id}";
    public const string CreateKnowledgeType = "create";
    public const string UpdateKnowledgeType = "update";
    public const string DeleteKnowledgeType = "delete/{id}";
    public const string AttachDetachKnowledges = "attach-detach-knowledges";

    #endregion
}
