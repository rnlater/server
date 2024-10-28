using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Domain.Base;
using Domain.Enums;

namespace Domain.Entities;

public class Material : BaseEntity
{
    public required MaterialType Type { get; set; }
    public required string Content { get; set; }
    public Guid KnowledgeId { get; set; }

    [ForeignKey("KnowledgeId")]
    public Knowledge? Knowledge { get; set; }

    public int? Order { get; set; }

    public Guid? ParentId { get; set; }

    [ForeignKey("ParentId")]
    public Material? Parent { get; set; }

    [InverseProperty("Parent")]
    public ICollection<Material> Children { get; set; } = [];

    public static Material ForTestPurposeOnly()
    {
        return new Material
        {
            Id = Guid.NewGuid(),
            Type = MaterialType.Text,
            Content = "Test Content",
            KnowledgeId = Guid.NewGuid()
        };
    }
}

