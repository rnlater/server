namespace Application.DTOs
{
    public class LearningListKnowledgeDto
    {
        public Guid LearningListId { get; set; }
        public LearningListDto? LearningList { get; set; }
        public Guid KnowledgeId { get; set; }
        public KnowledgeDto? Knowledge { get; set; }
        public int? Order { get; set; }
    }
}