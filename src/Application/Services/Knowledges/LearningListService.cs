using Application.DTOs;
using Application.Interfaces.Knowledges;
using Application.UseCases.Knowledges.LearningLists;
using Domain.Interfaces;
using Shared.Types;

namespace Application.Services.Knowledges
{
    public class LearningListService : ILearningListService
    {
        private readonly CreateLearningListUseCase _createLearningListUseCase;
        private readonly GetLearningListByGuidUseCase _getLearningListByGuidUseCase;
        private readonly GetAllLearningListsUseCase _getAllLearningListsUseCase;
        private readonly UpdateLearningListUseCase _updateLearningListUseCase;
        private readonly DeleteLearningListUseCase _deleteLearningListUseCase;
        private readonly AddRemoveKnowledgesToLearningListUseCase _AddRemoveKnowledgesToLearningListUseCase;

        public LearningListService(
            CreateLearningListUseCase createLearningListUseCase,
            GetLearningListByGuidUseCase getLearningListByGuidUseCase,
            GetAllLearningListsUseCase getAllLearningListsUseCase,
            UpdateLearningListUseCase updateLearningListUseCase,
            DeleteLearningListUseCase deleteLearningListUseCase,
            AddRemoveKnowledgesToLearningListUseCase AddRemoveKnowledgesToLearningListUseCase
        )
        {
            _createLearningListUseCase = createLearningListUseCase;
            _getLearningListByGuidUseCase = getLearningListByGuidUseCase;
            _getAllLearningListsUseCase = getAllLearningListsUseCase;
            _updateLearningListUseCase = updateLearningListUseCase;
            _deleteLearningListUseCase = deleteLearningListUseCase;
            _AddRemoveKnowledgesToLearningListUseCase = AddRemoveKnowledgesToLearningListUseCase;
        }

        public Task<Result<LearningListDto>> CreateLearningList(CreateLearningListParams parameters)
        {
            return _createLearningListUseCase.Execute(parameters);
        }

        public Task<Result<List<LearningListKnowledgeDto>>> AddRemoveKnowledgesToLearningList(AddRemoveKnowledgesToLearningListParams parameters)
        {
            return _AddRemoveKnowledgesToLearningListUseCase.Execute(parameters);
        }

        public Task<Result<IEnumerable<LearningListDto>>> GetAllLearningLists()
        {
            return _getAllLearningListsUseCase.Execute(NoParam.Value);
        }

        public Task<Result<LearningListDto>> GetLearningListByGuid(Guid guid)
        {
            return _getLearningListByGuidUseCase.Execute(guid);
        }

        public Task<Result<LearningListDto>> UpdateLearningList(UpdateLearningListParams Params)
        {
            return _updateLearningListUseCase.Execute(Params);
        }

        public Task<Result<LearningListDto>> DeleteLearningList(Guid guid)
        {
            return _deleteLearningListUseCase.Execute(guid);
        }
    }
}