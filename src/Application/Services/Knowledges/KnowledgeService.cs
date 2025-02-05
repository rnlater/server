using Application.DTOs;
using Application.DTOs.SingleIdPivotEntities;
using Application.Interfaces.Knowledges;
using Application.UseCases.Knowledges;
using Shared.Types;

namespace Application.Services.Knowledges
{
    public class KnowledgeService : IKnowledgeService
    {
        private readonly GetDetailedKnowledgeByGuidUseCase _getDetailedKnowledgeByGuidUseCase;
        private readonly SearchKnowledgesUseCase _searchKnowledgesUseCase;
        private readonly GetKnowledgesUseCase _getKnowledgesUseCase;
        private readonly CreateKnowledgeUseCase _createKnowledgeUseCase;
        private readonly UpdateKnowledgeUseCase _updateKnowledgeUseCase;
        private readonly DeleteKnowledgeUseCase _deleteKnowledgeUseCase;
        private readonly GetKnowledgesToLearnUseCase _getKnowledgesToLearnUseCase;
        private readonly GetCreatedKnowledgesUseCase _getCreatedKnowledgesUseCase;
        private readonly MigrateKnowledgesUseCase _migrateKnowledgesUseCase;

        public KnowledgeService(
            GetDetailedKnowledgeByGuidUseCase getDetailedKnowledgeByGuidUseCase,
            SearchKnowledgesUseCase searchKnowledgesUseCase,
            GetKnowledgesUseCase getKnowledgesUseCase,
            CreateKnowledgeUseCase createKnowledgeUseCase,
            UpdateKnowledgeUseCase updateKnowledgeUseCase,
            DeleteKnowledgeUseCase deleteKnowledgeUseCase,
            GetKnowledgesToLearnUseCase getKnowledgesToLearnUseCase,
            GetCreatedKnowledgesUseCase getCreatedKnowledgesUseCase,
            MigrateKnowledgesUseCase migrateKnowledgesUseCase
        )
        {
            _getDetailedKnowledgeByGuidUseCase = getDetailedKnowledgeByGuidUseCase;
            _searchKnowledgesUseCase = searchKnowledgesUseCase;
            _getKnowledgesUseCase = getKnowledgesUseCase;
            _createKnowledgeUseCase = createKnowledgeUseCase;
            _updateKnowledgeUseCase = updateKnowledgeUseCase;
            _deleteKnowledgeUseCase = deleteKnowledgeUseCase;
            _getKnowledgesToLearnUseCase = getKnowledgesToLearnUseCase;
            _getCreatedKnowledgesUseCase = getCreatedKnowledgesUseCase;
            _migrateKnowledgesUseCase = migrateKnowledgesUseCase;
        }

        public Task<Result<KnowledgeDto>> CreateKnowledge(CreateKnowledgeParams Params)
        {
            return _createKnowledgeUseCase.Execute(Params);
        }

        public Task<Result<KnowledgeDto>> DeleteKnowledge(Guid guid)
        {
            return _deleteKnowledgeUseCase.Execute(guid);
        }

        public Task<Result<KnowledgeDto>> GetDetailedKnowledgeByGuid(Guid guid)
        {
            return _getDetailedKnowledgeByGuidUseCase.Execute(guid);
        }

        public Task<Result<KnowledgeDto>> UpdateKnowledge(UpdateKnowledgeParams Params)
        {
            return _updateKnowledgeUseCase.Execute(Params);
        }

        public Task<Result<IEnumerable<KnowledgeDto>>> SearchKnowledges(SearchKnowledgesParams Params)
        {
            return _searchKnowledgesUseCase.Execute(Params);
        }

        public Task<Result<IEnumerable<KnowledgeDto>>> GetKnowledges(GetKnowledgesParams Params)
        {
            return _getKnowledgesUseCase.Execute(Params);
        }

        public Task<Result<List<List<KnowledgeDto>>>> GetKnowledgesToLearn(GetKnowledgesToLearnParams Params)
        {
            return _getKnowledgesToLearnUseCase.Execute(Params);
        }

        public Task<Result<IEnumerable<KnowledgeDto>>> GetCreatedKnowledges(GetCreatedKnowledgesParams Params)
        {
            return _getCreatedKnowledgesUseCase.Execute(Params);
        }

        public Task<Result<IEnumerable<LearningDto>>> MigrateKnowledges(MigrateKnowledgesParams Params)
        {
            return _migrateKnowledgesUseCase.Execute(Params);
        }
    }
}
