using Application.DTOs;
using Shared.Types;
using Application.UseCases.Knowledges.KnowledgeTopics;

namespace Application.Interfaces.Knowledges
{
    public interface IKnowledgeTopicService
    {
        /// <summary>
        /// Get knowledge topic by GUID.
        /// </summary>
        /// <param name="id">The GUID of the knowledge topic.</param>
        /// <returns>Returns the result containing the knowledge topic DTO if found, otherwise an error message.</returns>
        /// <exception cref="ErrorMessage.NoKnowledgeTopicFoundWithGuid">Thrown when no knowledge topic is found with the specified GUID.</exception>
        Task<Result<KnowledgeTopicDto>> GetKnowledgeTopicByGuid(Guid id);

        /// <summary>
        /// Get all knowledge topics.
        /// </summary>
        /// <returns>Returns the result containing a list of knowledge topic DTOs if found, otherwise an error message.</returns>
        /// <exception cref="ErrorMessage.NoKnowledgeTopicsFound">Thrown when no knowledge topics are found.</exception>
        Task<Result<IEnumerable<KnowledgeTopicDto>>> GetKnowledgeTopics(GetKnowledgeTopicsParams Params);

        /// <summary>
        /// Create a new knowledge topic.
        /// </summary>
        /// <param name="Params">The parameters for creating the knowledge topic.</param>
        /// <returns>Returns the result containing the created knowledge topic DTO if successful, otherwise an error message.</returns>
        /// <exception cref="ErrorMessage.KnowledgeTopicAlreadyExists">Thrown when a knowledge topic with the same name already exists.</exception>
        /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">Thrown when no knowledge is found with the specified GUID.</exception>
        /// <exception cref="ErrorMessage.NoKnowledgeTopicFoundWithGuid">Thrown when no knowledge topic is found with the specified GUID.</exception>
        Task<Result<KnowledgeTopicDto>> CreateKnowledgeTopic(CreateKnowledgeTopicParams Params);

        /// <summary>
        /// Update an existing knowledge topic.
        /// </summary>
        /// <param name="Params">The parameters for updating the knowledge topic.</param>
        /// <returns>Returns the result containing the updated knowledge topic DTO if successful, otherwise an error message.</returns>
        /// <exception cref="ErrorMessage.NoKnowledgeTopicFoundWithGuid">Thrown when no knowledge topic is found with the specified GUID.</exception>
        /// <exception cref="ErrorMessage.CannotBeParentOfItself">Thrown when a knowledge topic is attempted to be set as its own parent.</exception>
        Task<Result<KnowledgeTopicDto>> UpdateKnowledgeTopic(UpdateKnowledgeTopicParams Params);

        /// <summary>
        /// Delete a knowledge topic by GUID.
        /// </summary>
        /// <param name="id">The GUID of the knowledge topic to delete.</param>
        /// <returns>Returns the result containing the deleted knowledge topic DTO if successful, otherwise an error message.</returns>
        /// <exception cref="ErrorMessage.NoKnowledgeTopicFoundWithGuid">Thrown when no knowledge topic is found with the specified GUID.</exception>
        Task<Result<KnowledgeTopicDto>> DeleteKnowledgeTopic(Guid id);

        /// <summary>
        /// Attach or detach knowledges to/from a knowledge topic.
        /// </summary>
        /// <param name="Params">The parameters for attaching or detaching knowledges.</param>
        /// <returns>Returns the result indicating whether the operation was successful.</returns>
        /// <exception cref="ErrorMessage.NoKnowledgeTopicFoundWithGuid">Thrown when no knowledge topic is found with the specified GUID.</exception>
        /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">Thrown when no knowledge is found with the specified GUID.</exception>
        Task<Result<bool>> AttachDetachKnowledges(AttachDetachKnowledgesParams Params);
    }
}
