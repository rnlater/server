using Application.DTOs;
using Application.Interfaces.Knowledges;
using Application.UseCases.Knowledges.KnowledgeTopics;
using Shared.Types;

namespace Application.Services.Knowledges
{
    public class KnowledgeTopicService : IKnowledgeTopicService
    {
        private readonly GetKnowledgeTopicByGuidUseCase _getKnowledgeTopicByGuidUseCase;
        private readonly GetKnowledgeTopicsUseCase _getKnowledgeTopicsUseCase;
        private readonly CreateKnowledgeTopicUseCase _createKnowledgeTopicUseCase;
        private readonly UpdateKnowledgeTopicUseCase _updateKnowledgeTopicUseCase;
        private readonly DeleteKnowledgeTopicUseCase _deleteKnowledgeTopicUseCase;
        private readonly AttachDetachKnowledgesUseCase _attachDetachKnowledgesUseCase;
        private readonly GetTopicsForMigrationUseCase _getTopicsForMigrationUseCase;

        public KnowledgeTopicService(
            GetKnowledgeTopicByGuidUseCase GetKnowledgeTopicByGuidUseCase,
            GetKnowledgeTopicsUseCase getKnowledgeTopicsUseCase,
            CreateKnowledgeTopicUseCase createKnowledgeTopicUseCase,
            UpdateKnowledgeTopicUseCase updateKnowledgeTopicUseCase,
            DeleteKnowledgeTopicUseCase deleteKnowledgeTopicUseCase,
            AttachDetachKnowledgesUseCase attachDetachKnowledgesUseCase,
            GetTopicsForMigrationUseCase getTopicsForMigrationUseCase)
        {
            _getKnowledgeTopicByGuidUseCase = GetKnowledgeTopicByGuidUseCase;
            _getKnowledgeTopicsUseCase = getKnowledgeTopicsUseCase;
            _createKnowledgeTopicUseCase = createKnowledgeTopicUseCase;
            _updateKnowledgeTopicUseCase = updateKnowledgeTopicUseCase;
            _deleteKnowledgeTopicUseCase = deleteKnowledgeTopicUseCase;
            _attachDetachKnowledgesUseCase = attachDetachKnowledgesUseCase;
            _getTopicsForMigrationUseCase = getTopicsForMigrationUseCase;
        }

        public Task<Result<KnowledgeTopicDto>> GetKnowledgeTopicByGuid(Guid id)
        {
            return _getKnowledgeTopicByGuidUseCase.Execute(id);
        }

        public Task<Result<IEnumerable<KnowledgeTopicDto>>> GetKnowledgeTopics(GetKnowledgeTopicsParams Params)
        {
            return _getKnowledgeTopicsUseCase.Execute(Params);
        }

        public Task<Result<KnowledgeTopicDto>> CreateKnowledgeTopic(CreateKnowledgeTopicParams Params)
        {
            return _createKnowledgeTopicUseCase.Execute(Params);
        }

        public Task<Result<KnowledgeTopicDto>> UpdateKnowledgeTopic(UpdateKnowledgeTopicParams Params)
        {
            return _updateKnowledgeTopicUseCase.Execute(Params);
        }

        public Task<Result<KnowledgeTopicDto>> DeleteKnowledgeTopic(Guid id)
        {
            return _deleteKnowledgeTopicUseCase.Execute(id);
        }

        public Task<Result<bool>> AttachDetachKnowledges(AttachDetachKnowledgesParams Params)
        {
            return _attachDetachKnowledgesUseCase.Execute(Params);
        }

        public Task<Result<IEnumerable<KnowledgeTopicDto>>> GetTopicsForMigration(GetTopicsForMigrationParams Params)
        {
            return _getTopicsForMigrationUseCase.Execute(Params);
        }
    }
}
