namespace Application.DTOs.PivotEntities;

public class KnowledgeTopicKnowledgeDto
{
    public Guid KnowledgeTopicId { get; set; }
    public KnowledgeTopicDto? KnowledgeTopic { get; set; }

    public Guid KnowledgeId { get; set; }
    public KnowledgeDto? Knowledge { get; set; }
}
