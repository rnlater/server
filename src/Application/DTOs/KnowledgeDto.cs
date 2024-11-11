using System.ComponentModel.DataAnnotations;
using Application.DTOs.PivotEntities;
using Domain.Enums;

namespace Application.DTOs;

public class KnowledgeDto : SingleIdEntityDto
{
    public required string Title { get; set; }
    [EnumDataType(typeof(KnowledgeVisibility))]
    public required string Visibility { get; set; }
    [EnumDataType(typeof(KnowledgeLevel))]
    public required string Level { get; set; }
    public Guid CreatorId { get; set; }
    public UserDto? Creator { get; set; }
    public ICollection<MaterialDto> Materials { get; set; } = [];
    public ICollection<SubjectKnowledgeDto> SubjectKnowledges { get; set; } = [];
    public ICollection<KnowledgeTypeKnowledgeDto> KnowledgeTypeKnowledges { get; set; } = [];
    public ICollection<KnowledgeTopicKnowledgeDto> KnowledgeTopicKnowledges { get; set; } = [];
}
