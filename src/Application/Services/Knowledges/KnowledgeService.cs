using Application.DTOs;
using Application.Interfaces.Knowledges;
using Application.UseCases.Knowledges;
using Shared.Types;

namespace Application.Services.Knowledges
{
    public class KnowledgeService : IKnowledgeService
    {
        private readonly GetDetailedKnowledgeByGuidUseCase _getDetailedKnowledgeByGuidUseCase;
        private readonly SearchKnowledgesUseCase _searchKnowledgesUseCase;

        public KnowledgeService(
            GetDetailedKnowledgeByGuidUseCase getDetailedKnowledgeByGuidUseCase,
            SearchKnowledgesUseCase searchKnowledgesUseCase)
        {
            _getDetailedKnowledgeByGuidUseCase = getDetailedKnowledgeByGuidUseCase;
            _searchKnowledgesUseCase = searchKnowledgesUseCase;
        }

        public Task<Result<KnowledgeDto>> GetDetailedKnowledgeByGuid(Guid id)
        {
            return _getDetailedKnowledgeByGuidUseCase.Execute(id);
        }

        Task<Result<IEnumerable<KnowledgeDto>>> IKnowledgeService.SearchKnowledges(SearchKnowledgesParameters Params)
        {
            return _searchKnowledgesUseCase.Execute(Params);
        }
    }
}
