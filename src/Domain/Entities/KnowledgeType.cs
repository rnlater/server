using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.Entities;

public class KnowledgeType : BaseEntity
{
    public required string Name { get; set; }

    public Guid? ParentId { get; set; }

    [ForeignKey("ParentId")]
    public KnowledgeType? Parent { get; set; }

    [InverseProperty("Parent")]
    public ICollection<KnowledgeType> Children { get; set; } = [];

    public ICollection<Knowledge> Knowledges { get; set; } = [];

    public static KnowledgeType ForTestPurposeOnly()
    {
        return new KnowledgeType
        {
            Id = Guid.NewGuid(),
            Name = "Test KnowledgeType",
        };
    }

}
