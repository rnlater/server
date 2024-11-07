namespace Application.DTOs.PivotEntities;

public class KnowledgeTypeKnowledgeDto
{
    public Guid KnowledgeTypeId { get; set; }
    public KnowledgeTypeDto? KnowledgeType { get; set; }

    public Guid KnowledgeId { get; set; }
    public KnowledgeDto? Knowledge { get; set; }
}
