using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.PivotEntities;

namespace Domain.Entities.SingleIdEntities;

public class KnowledgeType : SingleIdEntity
{
    public required string Name { get; set; }

    public Guid? ParentId { get; set; }

    [ForeignKey("ParentId")]
    public KnowledgeType? Parent { get; set; }

    [InverseProperty("Parent")]
    public ICollection<KnowledgeType> Children { get; set; } = [];

    public ICollection<KnowledgeTypeKnowledge> KnowledgeTypeKnowledges { get; set; } = [];

    public static KnowledgeType ForTestPurposeOnly()
    {
        return new KnowledgeType
        {
            Id = Guid.NewGuid(),
            Name = "Test KnowledgeType",
        };
    }

}
