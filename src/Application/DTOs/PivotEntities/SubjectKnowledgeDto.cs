namespace Application.DTOs.PivotEntities;

public class SubjectKnowledgeDto
{
    public Guid SubjectId { get; set; }
    public SubjectDto? Subject { get; set; }

    public Guid KnowledgeId { get; set; }
    public KnowledgeDto? Knowledge { get; set; }
}
