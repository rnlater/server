using Application.DTOs;
using Application.Interfaces.Knowledges;
using Application.UseCases.Knowledges.KnowledgeTypes;
using Application.UseCases.KnowledgeTypes;
using Domain.Interfaces;
using Shared.Types;

namespace Application.Services.Knowledges
{
    public class KnowledgeTypeService : IKnowledgeTypeService
    {
        private readonly GetKnowledgeTypeByGuidUseCase _getKnowledgeTypeByIdUseCase;
        private readonly GetKnowledgeTypesUseCase _getKnowledgeTypesUseCase;
        private readonly CreateKnowledgeTypeUseCase _createKnowledgeTypeUseCase;
        private readonly UpdateKnowledgeTypeUseCase _updateKnowledgeTypeUseCase;
        private readonly DeleteKnowledgeTypeUseCase _deleteKnowledgeTypeUseCase;
        private readonly AttachDetachKnowledgesUseCase _attachDetachKnowledgesUseCase;

        public KnowledgeTypeService(
            GetKnowledgeTypeByGuidUseCase getKnowledgeTypeByIdUseCase,
            GetKnowledgeTypesUseCase getKnowledgeTypesUseCase,
            CreateKnowledgeTypeUseCase createKnowledgeTypeUseCase,
            UpdateKnowledgeTypeUseCase updateKnowledgeTypeUseCase,
            DeleteKnowledgeTypeUseCase deleteKnowledgeTypeUseCase,
            AttachDetachKnowledgesUseCase attachDetachKnowledgesUseCase)
        {
            _getKnowledgeTypeByIdUseCase = getKnowledgeTypeByIdUseCase;
            _getKnowledgeTypesUseCase = getKnowledgeTypesUseCase;
            _createKnowledgeTypeUseCase = createKnowledgeTypeUseCase;
            _updateKnowledgeTypeUseCase = updateKnowledgeTypeUseCase;
            _deleteKnowledgeTypeUseCase = deleteKnowledgeTypeUseCase;
            _attachDetachKnowledgesUseCase = attachDetachKnowledgesUseCase;
        }

        public Task<Result<KnowledgeTypeDto>> GetKnowledgeTypeByGuid(Guid id)
        {
            return _getKnowledgeTypeByIdUseCase.Execute(id);
        }

        public Task<Result<IEnumerable<KnowledgeTypeDto>>> GetKnowledgeTypes()
        {
            return _getKnowledgeTypesUseCase.Execute(NoParam.Value);
        }

        public Task<Result<KnowledgeTypeDto>> CreateKnowledgeType(CreateKnowledgeTypeParams Params)
        {
            return _createKnowledgeTypeUseCase.Execute(Params);
        }

        public Task<Result<KnowledgeTypeDto>> UpdateKnowledgeType(UpdateKnowledgeTypeParams Params)
        {
            return _updateKnowledgeTypeUseCase.Execute(Params);
        }

        public Task<Result<KnowledgeTypeDto>> DeleteKnowledgeType(Guid id)
        {
            return _deleteKnowledgeTypeUseCase.Execute(id);
        }

        public Task<Result<bool>> AttachDetachKnowledges(AttachDetachKnowledgesParams Params)
        {
            return _attachDetachKnowledgesUseCase.Execute(Params);
        }

    }
}
