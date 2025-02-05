using Application.DTOs;
using Application.UseCases.Knowledges.LearningLists;
using Shared.Types;

namespace Application.Interfaces.Knowledges
{
    public interface ILearningListService
    {
        /// <summary>
        /// Get all learning lists of the authenticated user.
        /// </summary>
        /// <returns>Result containing a list of learning lists of the current user.</returns>
        /// <exception cref="ErrorMessage.UserNotFound">User not found</exception>
        Task<Result<IEnumerable<LearningListDto>>> GetAllLearningLists();

        /// <summary>
        /// Get a learning list by its GUID.
        /// </summary>
        /// <param name="guid">The GUID of the learning list.</param>
        /// <returns>Result containing the learning list with the specified GUID.</returns>
        /// <exception cref="ErrorMessage.NoLearningListFoundWithGuid">No learning list found with the specified GUID</exception>
        Task<Result<LearningListDto>> GetLearningListByGuid(Guid guid);

        /// <summary>
        /// Create a new learning list.
        /// </summary>
        /// <param name="Params">Parameters for creating the learning list.</param>
        /// <returns>Result containing the created learning list.</returns>
        /// <exception cref="ErrorMessage.LearningListTitleExisted">Learning list title already exists</exception>
        /// <exception cref="ErrorMessage.UserNotFound">User not found</exception>
        Task<Result<LearningListDto>> CreateLearningList(CreateLearningListParams Params);

        /// <summary>
        /// Update an existing learning list.
        /// </summary>
        /// <param name="Params">Parameters for updating the learning list.</param>
        /// <returns>Result containing the updated learning list.</returns>
        /// <exception cref="ErrorMessage.NoLearningListFoundWithGuid">No learning list found with the specified GUID</exception>
        /// <exception cref="ErrorMessage.UserNotAuthorized">User not authorized to update the learning list</exception>
        /// <exception cref="ErrorMessage.LearningListTitleExisted">Learning list title already exists</exception>
        Task<Result<LearningListDto>> UpdateLearningList(UpdateLearningListParams Params);

        /// <summary>
        /// Delete a learning list by its GUID.
        /// </summary>
        /// <param name="guid">The GUID of the learning list to delete.</param>
        /// <returns>Result containing the deleted learning list.</returns>
        /// <exception cref="ErrorMessage.NoLearningListFoundWithGuid">No learning list found with the specified GUID</exception>
        /// <exception cref="ErrorMessage.UserNotAuthorized">User not authorized to delete the learning list</exception>
        Task<Result<LearningListDto>> DeleteLearningList(Guid guid);

        /// <summary>
        /// Add or remove knowledges to/from a learning list.
        /// </summary>
        /// <param name="Params">Parameters for adding or removing knowledges.</param>
        /// <returns>Result containing the updated list of learning list knowledges.</returns>
        /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">No knowledge found with the specified GUID</exception>
        /// <exception cref="ErrorMessage.UserNotAuthorized">User not authorized to modify the learning list</exception>
        Task<Result<List<LearningListKnowledgeDto>>> AddRemoveKnowledgesToLearningList(AddRemoveKnowledgesToLearningListParams Params);
    }
}