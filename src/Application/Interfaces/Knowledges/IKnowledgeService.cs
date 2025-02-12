using Application.DTOs;
using Application.DTOs.SingleIdPivotEntities;
using Application.UseCases.Knowledges;
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
    /// Get knowledges to learn
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of grouped knowledges to learn</returns>
    /// <exception cref="ErrorMessage.UserNotFound">User not found</exception>
    /// <exception cref="ErrorMessage.KnowledgeAlreadyLearned">Knowledge already learned</exception>
    /// <exception cref="ErrorMessage.SomeKnowledgesNotFound">Some knowledges not found</exception>
    /// <exception cref="ErrorMessage.RequireTwoGamesToLearn">Require a game to review</exception>
    Task<Result<List<List<KnowledgeDto>>>> GetKnowledgesToLearn(GetKnowledgesToLearnParams Params);

    /// <summary>
    /// Get knowledges with parameters
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of knowledges with related entities</returns>
    /// <exception cref="ErrorMessage.NoKnowledgesFound">No knowledges found</exception>
    Task<Result<IEnumerable<KnowledgeDto>>> GetKnowledges(GetKnowledgesParams Params);

    /// <summary>
    /// Get user's created knowledges
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of created knowledges with related entities</returns>
    /// <exception cref="ErrorMessage.NoKnowledgesFound">No knowledges found</exception>
    Task<Result<IEnumerable<KnowledgeDto>>> GetCreatedKnowledges(GetCreatedKnowledgesParams Params);

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
    /// Allow user to choose known knowledges
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of learning for the knowledges</returns>
    /// <exception cref="ErrorMessage.UserNotFound">User not found</exception>
    /// <exception cref="ErrorMessage.SomeKnowledgesNotFound">Some knowledges not found</exception>
    Task<Result<IEnumerable<LearningDto>>> MigrateKnowledges(MigrateKnowledgesParams Params);
}
