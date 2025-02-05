using Application.DTOs;
using Application.UseCases.Knowledges.KnowledgeTypes;
using Shared.Types;

namespace Application.Interfaces.Knowledges
{
    public interface IKnowledgeTypeService
    {
        /// <summary>
        /// Get knowledge type by guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return result of knowledge type dto with its parent, children and knowledges</returns>
        /// <exception cref="ErrorMessage.NoKnowledgeTypeFoundWithGuid">No knowledge type found with guid</exception>
        Task<Result<KnowledgeTypeDto>> GetKnowledgeTypeByGuid(Guid id);

        /// <summary>
        /// Get knowledge types
        /// </summary>
        /// <returns>return result of list of knowledge type dto with its parent, children and knowledges</returns>
        /// <exception cref="ErrorMessage.NoKnowledgeTypesFound">No knowledge types found</exception>
        Task<Result<IEnumerable<KnowledgeTypeDto>>> GetKnowledgeTypes(GetKnowledgeTypesParams Params);

        /// <summary>
        /// Create knowledge type
        /// </summary>
        /// <param name="Params"></param>
        /// <returns>return result of knowledge type dto with its parent, children and knowledges</returns>
        /// <exception cref="ErrorMessage.KnowledgeTypeAlreadyExists">Knowledge type already exists</exception>
        /// <exception cref="ErrorMessage.NoKnowledgeTypeFoundWithGuid">No knowledge type found with guid</exception>
        /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">No knowledge found with guid</exception>
        Task<Result<KnowledgeTypeDto>> CreateKnowledgeType(CreateKnowledgeTypeParams Params);

        /// <summary>
        /// Update knowledge type
        /// </summary>
        /// <param name="Params"></param>
        /// <returns>return result of knowledge type dto with its parent, children and knowledges</returns>
        /// <exception cref="ErrorMessage.NoKnowledgeTypeFoundWithGuid">No knowledge type found with guid</exception>
        Task<Result<KnowledgeTypeDto>> UpdateKnowledgeType(UpdateKnowledgeTypeParams Params);

        /// <summary>
        /// Delete knowledge type
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return result of knowledge type dto with its parent, children and knowledges</returns>
        /// <exception cref="ErrorMessage.NoKnowledgeTypeFoundWithGuid">No knowledge type found with guid</exception>
        Task<Result<KnowledgeTypeDto>> DeleteKnowledgeType(Guid id);

        /// <summary>
        /// Attach detach knowledges
        /// </summary>
        /// <param name="Params"></param>
        /// <returns>return result of pivot success modification type</returns>
        /// <exception cref="ErrorMessage.NoKnowledgeTypeFoundWithGuid">No knowledge type found with guid</exception>
        /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">No knowledge found with guid</exception>
        Task<Result<bool>> AttachDetachKnowledges(AttachDetachKnowledgesParams Params);
    }
}
