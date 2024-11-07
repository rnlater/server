using Application.DTOs.PivotEntities;

namespace Application.DTOs;

public class KnowledgeTypeDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid? ParentId { get; set; }
    public KnowledgeTypeDto? Parent { get; set; }
    public ICollection<KnowledgeTypeDto> Children { get; set; } = [];
    public ICollection<KnowledgeTypeKnowledgeDto> KnowledgeTypeKnowledges { get; set; } = [];

}