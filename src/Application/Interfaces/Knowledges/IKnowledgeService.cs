using Application.DTOs;
using Application.UseCases.Knowledges;
using Domain.Interfaces;
using Shared.Types;

namespace Application.Interfaces.Knowledges;

public interface IKnowledgeService
{
    /// <summary>
    /// Get knowledge by guid
    /// </summary>
    /// <param name="guid"></param>
    /// <returns>return result of knowledge with related entities</returns>
    /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">No knowledge found with guid</exception>
    Task<Result<KnowledgeDto>> GetDetailedKnowledgeByGuid(Guid guid);

    /// <summary>
    /// Search knowledges with parameters
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of knowledges with related entities</returns>
    /// <exception cref="ErrorMessage.NoKnowledgeTypesFound">No knowledge types found</exception>
    /// <exception cref="ErrorMessage.SomeKnowledgeTypesNotFound">Some knowledge types not found</exception>
    /// <exception cref="ErrorMessage.NoKnowledgeTopicsFound">No knowledge topics found</exception>
    /// <exception cref="ErrorMessage.SomeKnowledgeTopicsNotFound">Some knowledge topics not found</exception>
    /// <exception cref="ErrorMessage.NoKnowledgesFound">No knowledges found</exception>
    Task<Result<IEnumerable<KnowledgeDto>>> SearchKnowledges(SearchKnowledgesParams Params);

    /// <summary>
    /// Get knowledges with parameters
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of knowledges with related entities</returns>
    /// <exception cref="ErrorMessage.NoKnowledgesFound">No knowledges found</exception>
    Task<Result<IEnumerable<KnowledgeDto>>> GetKnowledges(GetKnowledgesParams Params);

    /// <summary>
    /// Create knowledge
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of created knowledge</returns>
    /// <exception cref="ErrorMessage.NoKnowledgeTypesFound">No knowledge types found</exception>
    /// <exception cref="ErrorMessage.NoKnowledgeTopicsFound">No knowledge topics found</exception>
    /// <exception cref="ErrorMessage.NoSubjectsFound">No subjects found</exception>
    Task<Result<KnowledgeDto>> CreateKnowledge(CreateKnowledgeParams Params);

    /// <summary>
    /// Update knowledge
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of updated knowledge</returns>
    /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">No knowledge found with guid</exception>
    Task<Result<KnowledgeDto>> UpdateKnowledge(UpdateKnowledgeParams Params);

    /// <summary>
    /// Delete knowledge
    /// </summary>
    /// <param name="guid"></param>
    /// <returns>return result of deleted knowledge</returns>
    /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">No knowledge found with guid</exception>
    Task<Result<KnowledgeDto>> DeleteKnowledge(Guid guid);

    /// <summary>
    /// Attach or deattach knowledge type
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of attached or deattached knowledge type</returns>
    /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">No knowledge found with guid</exception>
    /// <exception cref="ErrorMessage.NoKnowledgeTypeFoundWithGuid">No knowledge type found with guid</exception>
    Task<Result<PivotSuccessModificationType>> AttachDeattachKnowledgeType(AttachDeattachKnowledgeTypeParams Params);

    /// <summary>
    /// Attach or deattach knowledge topic
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of attached or deattached knowledge topic</returns>
    /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">No knowledge found with guid</exception>
    /// <exception cref="ErrorMessage.NoKnowledgeTopicFoundWithGuid">No knowledge topic found with guid</exception>
    Task<Result<PivotSuccessModificationType>> AttachDeattachKnowledgeTopic(AttachDeattachKnowledgeTopicParams Params);

    /// <summary>
    /// Publish knowledge
    /// </summary>
    /// <param name="guid"></param>
    /// <returns>return result of published knowledge</returns>
    /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">No knowledge found with guid</exception>
    Task<Result<KnowledgeDto>> PublishKnowledge(Guid guid);
}
