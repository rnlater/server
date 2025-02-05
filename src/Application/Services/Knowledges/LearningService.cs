using Application.DTOs.SingleIdPivotEntities;
using Shared.Types;
using Application.Interfaces.Knowledges;
using Application.UseCases.Knowledges.Learnings;
using Domain.Interfaces;

namespace Application.Services.Knowledges
{
    public class LearningService : ILearningService
    {
        private readonly LearnKnowledgeUseCase _learnKnowledgeUseCase;
        private readonly GetLearningsToReviewUseCase _getLearningsToReviewUseCase;
        private readonly ReviewLearningUseCase _reviewLearningUseCase;
        private readonly GetCurrentUserLearningsUseCase _getCurrentUserLearningsUseCase;
        private readonly GetUnlistedLearningsUseCase _getUnlistedLearningsUseCase;

        public LearningService(
            LearnKnowledgeUseCase learnKnowledgeUseCase,
            GetLearningsToReviewUseCase getLearningsToReviewUseCase,
            ReviewLearningUseCase reviewLearningUseCase,
            GetCurrentUserLearningsUseCase getCurrentUserLearningsUseCase,
            GetUnlistedLearningsUseCase getUnlistedLearningsUseCase)
        {
            _learnKnowledgeUseCase = learnKnowledgeUseCase;
            _getLearningsToReviewUseCase = getLearningsToReviewUseCase;
            _reviewLearningUseCase = reviewLearningUseCase;
            _getCurrentUserLearningsUseCase = getCurrentUserLearningsUseCase;
            _getUnlistedLearningsUseCase = getUnlistedLearningsUseCase;
        }

        public Task<Result<List<LearningDto>>> LearnKnowledge(List<LearnKnowledgeParams> Params)
        {
            return _learnKnowledgeUseCase.Execute(Params);
        }

        public Task<Result<List<List<LearningDto>>>> GetLearningsToReview(GetLearningsToReviewParams Params)
        {
            return _getLearningsToReviewUseCase.Execute(Params);
        }

        public Task<Result<List<LearningDto>>> ReviewLearning(List<ReviewLearningParams> Params)
        {
            return _reviewLearningUseCase.Execute(Params);
        }

        public Task<Result<List<LearningDto>>> GetCurrentUserLearnings(GetCurrentUserLearningsParams Params)
        {
            return _getCurrentUserLearningsUseCase.Execute(Params);
        }

        public Task<Result<List<LearningDto>>> GetUnlistedLearnings()
        {
            return _getUnlistedLearningsUseCase.Execute(NoParam.Value);
        }
    }
}