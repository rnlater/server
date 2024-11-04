using Application.DTOs.PivotEntities;

namespace Application.DTOs;

public class SubjectDto : SingleIdEntityDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Photo { get; set; }
    public IEnumerable<TrackSubjectDto> Tracks { get; set; } = [];
    public IEnumerable<SubjectKnowledgeDto> Knowledges { get; set; } = [];
    public int KnowledgeCount => Knowledges.Count();
}
