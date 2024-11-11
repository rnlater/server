using Application.DTOs;
using Application.UseCases.Knowledges;
using Shared.Types;

namespace Application.Interfaces.Knowledges;

public interface IKnowledgeService
{
    Task<Result<KnowledgeDto>> GetDetailedKnowledgeByGuid(Guid id);
    Task<Result<IEnumerable<KnowledgeDto>>> SearchKnowledges(SearchKnowledgesParameters Params);
}
