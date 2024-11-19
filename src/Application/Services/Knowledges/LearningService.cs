using Application.DTOs.SingleIdPivotEntities;
using Shared.Types;
using Application.Interfaces.Knowledges;
using Application.UseCases.Knowledges.Learnings;

namespace Application.Services.Knowledges
{
    public class LearningService : ILearningService
    {
        private readonly LearnKnowledgeUseCase _learnKnowledgeUseCase;
        private readonly GetLearningsToReviewUseCase _getLearningsToReviewUseCase;
        private readonly ReviewLearningUseCase _reviewLearningUseCase;

        public LearningService(
            LearnKnowledgeUseCase learnKnowledgeUseCase,
            GetLearningsToReviewUseCase getLearningsToReviewUseCase,
            ReviewLearningUseCase reviewLearningUseCase)
        {
            _learnKnowledgeUseCase = learnKnowledgeUseCase;
            _getLearningsToReviewUseCase = getLearningsToReviewUseCase;
            _reviewLearningUseCase = reviewLearningUseCase;
        }

        public Task<Result<Dictionary<Guid, int>>> LearnKnowledge(List<LearnKnowledgeParams> Params)
        {
            return _learnKnowledgeUseCase.Execute(Params);
        }

        public Task<Result<List<Dictionary<Guid, LearningDataToReview>>>> GetLearningsToReview(GetLearningsToReviewParams Params)
        {
            return _getLearningsToReviewUseCase.Execute(Params);
        }

        public Task<Result<List<LearningDto>>> ReviewLearning(List<ReviewLearningParams> Params)
        {
            return _reviewLearningUseCase.Execute(Params);
        }
    }
}