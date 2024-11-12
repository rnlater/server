using Application.DTOs;
using Application.Interfaces.Knowledges;
using Application.UseCases.Knowledges;
using Domain.Interfaces;
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
        private readonly AttachDeattachKnowledgeTypeUseCase _attachDeattachKnowledgeTypeUseCase;
        private readonly AttachDeattachKnowledgeTopicUseCase _attachDeattachKnowledgeTopicUseCase;
        private readonly PublishKnowledgeUseCase _publishKnowledgeUseCase;

        public KnowledgeService(
            GetDetailedKnowledgeByGuidUseCase getDetailedKnowledgeByGuidUseCase,
            SearchKnowledgesUseCase searchKnowledgesUseCase,
            GetKnowledgesUseCase getKnowledgesUseCase,
            CreateKnowledgeUseCase createKnowledgeUseCase,
            UpdateKnowledgeUseCase updateKnowledgeUseCase,
            DeleteKnowledgeUseCase deleteKnowledgeUseCase,
            AttachDeattachKnowledgeTypeUseCase attachDeattachKnowledgeTypeUseCase,
            AttachDeattachKnowledgeTopicUseCase attachDeattachKnowledgeTopicUseCase,
            PublishKnowledgeUseCase publishKnowledgeUseCase
        )
        {
            _getDetailedKnowledgeByGuidUseCase = getDetailedKnowledgeByGuidUseCase;
            _searchKnowledgesUseCase = searchKnowledgesUseCase;
            _getKnowledgesUseCase = getKnowledgesUseCase;
            _createKnowledgeUseCase = createKnowledgeUseCase;
            _updateKnowledgeUseCase = updateKnowledgeUseCase;
            _deleteKnowledgeUseCase = deleteKnowledgeUseCase;
            _attachDeattachKnowledgeTypeUseCase = attachDeattachKnowledgeTypeUseCase;
            _attachDeattachKnowledgeTopicUseCase = attachDeattachKnowledgeTopicUseCase;
            _publishKnowledgeUseCase = publishKnowledgeUseCase;
        }

        public Task<Result<PivotSuccessModificationType>> AttachDeattachKnowledgeTopic(AttachDeattachKnowledgeTopicParams Params)
        {
            return _attachDeattachKnowledgeTopicUseCase.Execute(Params);
        }

        public Task<Result<PivotSuccessModificationType>> AttachDeattachKnowledgeType(AttachDeattachKnowledgeTypeParams Params)
        {
            return _attachDeattachKnowledgeTypeUseCase.Execute(Params);
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

        public Task<Result<KnowledgeDto>> PublishKnowledge(Guid guid)
        {
            return _publishKnowledgeUseCase.Execute(guid);
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
    }
}
