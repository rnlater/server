using Application.DTOs.SingleIdPivotEntities;

namespace Application.DTOs
{
    public class LearningListDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public Guid LearnerId { get; set; }
        public UserDto? Learner { get; set; }
        public ICollection<LearningListKnowledgeDto> LearningListKnowledges { get; set; } = [];
        public ICollection<LearningDto> LearntKnowledges { get; set; } = [];
        public ICollection<KnowledgeDto> NotLearntKnowledges { get; set; } = [];
    }
}