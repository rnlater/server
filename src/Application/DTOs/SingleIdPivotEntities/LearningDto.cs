namespace Application.DTOs.SingleIdPivotEntities
{
    public class LearningDto : SingleIdPivotEntityDto
    {
        public Guid KnowledgeId { get; set; }
        public KnowledgeDto? Knowledge { get; set; }
        public Guid UserId { get; set; }
        public UserDto? User { get; set; }
        public DateTime NextReviewDate { get; set; }
        public ICollection<LearningHistoryDto> LearningHistories { get; set; } = [];
    }
}