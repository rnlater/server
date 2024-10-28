using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Domain.Base;

namespace Domain.Entities;

public class KnowledgeTopic : BaseEntity
{
    public required string Title { get; set; }

    public int? Order { get; set; }

    public Guid? ParentId { get; set; }

    [ForeignKey("ParentId")]
    public KnowledgeTopic? Parent { get; set; }

    [InverseProperty("Parent")]
    public ICollection<KnowledgeTopic> Children { get; set; } = [];

    public ICollection<Knowledge> Knowledges { get; set; } = [];

    public static KnowledgeTopic ForTestPurposeOnly()
    {
        return new KnowledgeTopic
        {
            Id = Guid.NewGuid(),
            Title = "Test KnowledgeTopic",
        };
    }
}
