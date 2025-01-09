using Application.DTOs.SingleIdPivotEntities;
using Application.UseCases.Knowledges.Learnings;
using Shared.Types;

namespace Application.Interfaces.Knowledges
{
    public interface ILearningService
    {
        /// <summary>
        /// Learn knowledges
        /// </summary>
        /// <param name="Params"></param>
        /// <returns>return result of learnings</returns>
        /// <exception cref="ErrorMessage.SomeKnowledgesNotFound">Some knowledges not found</exception>
        /// <exception cref="ErrorMessage.UserNotFound">User not found</exception>
        /// <exception cref="ErrorMessage.SomeKnowledgesAlreadyLearned">Some knowledges already learned</exception>
        /// <exception cref="ErrorMessage.InvalidData">Invalid answer request</exception>
        Task<Result<List<LearningDto>>> LearnKnowledge(List<LearnKnowledgeParams> Params);

        /// <summary>
        /// Get learnings to review
        /// </summary>
        /// <param name="Params"></param>
        /// <returns>return learnings to review</returns>
        /// <exception cref="ErrorMessage.UserNotFound">User not found</exception>
        /// <exception cref="ErrorMessage.SomeKnowledgesHaveNotBeenLearned">Some knowledges have not been learned</exception>
        /// <exception cref="ErrorMessage.SomeKnowledgesAreNotReadyToReview">Some knowledges are not ready to review</exception>
        /// <exception cref="ErrorMessage.RequireAGameToReview">Each knowledge requires a game to review</exception>
        Task<Result<List<List<LearningDto>>>> GetLearningsToReview(GetLearningsToReviewParams Params);

        /// <summary>
        /// Review learning
        /// </summary>
        /// <param name="Params"></param>
        /// <returns>return result of review learning</returns>
        /// <exception cref="ErrorMessage.UserNotFound">User not found</exception>
        /// <exception cref="ErrorMessage.LearningNotFound">Learning not found</exception>
        /// <exception cref="ErrorMessage.KnowledgeNotReadyToReview">Knowledge not ready to review</exception>
        /// <exception cref="ErrorMessage.RequireLearningBeforeReview">Require learning before review</exception>
        /// <exception cref="ErrorMessage.InvalidData">Invalid answer request</exception>
        Task<Result<List<LearningDto>>> ReviewLearning(List<ReviewLearningParams> Params);

        /// <summary>
        /// Retrieves the current user's learnings, including both learnt and unlisted learnings.
        /// </summary>
        /// <param name="parameters">The parameters for retrieving the current user's learnings, including an optional search term.</param>
        /// <returns>A result containing the current user's learnings, or an error message if the operation fails.</returns>
        /// <exception cref="ErrorMessage.UserNotFound">User not found</exception>
        /// <exception cref="ErrorMessage.NoData">No learning found for the specific paramaters</exception>
        Task<Result<List<LearningDto>>> GetCurrentUserLearnings(GetCurrentUserLearningsParams Params); // TODO
        Task<Result<List<LearningDto>>> GetUnlistedLearnings(); // TODO
    }
}