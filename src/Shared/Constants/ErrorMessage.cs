namespace Shared.Constants;

public enum ErrorMessage
{
    UnknownError,
    NoChangeDetected,
    InvalidData,

    #region File

    StoreFileError,
    DeleteFileError,


    #endregion

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
    UserNotAuthorized,

    #endregion

    #region Authorization

    UserIsNotAdmin,

    #endregion

    #region Track

    NoTracksFound,
    NoTrackFoundWithSearch,
    NoTrackFoundWithGuid,

    #endregion

    #region Subject

    NoSubjectsFound,
    NoSubjectFoundWithGuid,

    #endregion

    #region Knowledge

    NoKnowledgesFound,
    NoKnowledgeFoundWithGuid,
    SomeKnowledgesNotFound,

    NoKnowledgeTypesFound,
    NoKnowledgeTypeFoundWithGuid,
    SomeKnowledgeTypesNotFound,
    KnowledgeTypeAlreadyExists,
    CannotBeParentOfItself,

    NoKnowledgeTopicFoundWithGuid,
    SomeKnowledgeTopicsNotFound,
    NoKnowledgeTopicsFound,
    KnowledgeTopicAlreadyExists,

    KnowledgeAlreadyLearned,
    SomeKnowledgesAlreadyLearned,
    SomeKnowledgesHaveNotBeenLearned,
    SomeKnowledgesAreNotReadyToReview,
    KnowledgeNotReadyToReview,
    LearningNotFound,
    RequireLearningBeforeReview,

    RequireExactTwoGames,
    RequireAGameToReview,
    GameKnowledgeSubscriptionNotFound,
    GameKnowledgeSubscriptionAlreadyExists,
    NoGameFoundWithGuid,
    NoGamesFound,

    GameOptionGroupNotFound,
    GameOptionNotFoundWithGuid,
    RequireExactOneQuestion,
    RequireAtLeastTwoAnswers,
    RequireExactOneCorrectAnswer,
    CannotDeleteCorrectAnswer,

    LearningListTitleExisted,
    NoLearningListFoundWithGuid,

    NoPublicationRequestFoundWithGuid,
    KnowledgeAlreadyRequestedForPublication,
    PublicationRequestAlreadyApproved,
    PublicationRequestAlreadyApprovedOrRejected,
    NoPublicationRequestsFound,

    #endregion
}
