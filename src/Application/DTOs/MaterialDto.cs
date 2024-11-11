using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs;

public class MaterialDto : SingleIdEntityDto
{
    [EnumDataType(typeof(MaterialType))]
    public required string Type { get; set; }
    public required string Content { get; set; }
    public Guid KnowledgeId { get; set; }
    public int? Order { get; set; }
    public Guid? ParentId { get; set; }
    public ICollection<MaterialDto> Children { get; set; } = [];
}
