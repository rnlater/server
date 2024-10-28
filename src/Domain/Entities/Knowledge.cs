using System;

namespace Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;
using Domain.Enums;

public class Knowledge : BaseEntity
{
    public required string Title { get; set; }
    public KnowledgeVisibility Visibility { get; set; }
    public KnowledgeLevel Level { get; set; }
    public Guid CreatorId { get; set; }

    [ForeignKey("CreatorId")]
    public User? Creator { get; set; }

    public ICollection<Subject> Subjects { get; set; } = [];
    public ICollection<Material> Materials { get; set; } = [];
    public ICollection<KnowledgeType> KnowledgeTypes { get; set; } = [];
    public ICollection<KnowledgeTopic> KnowledgeTopics { get; set; } = [];

    public static Knowledge ForTestPurposeOnly()
    {
        return new Knowledge
        {
            Id = Guid.NewGuid(),
            Title = "Test Knowledge",
            Visibility = KnowledgeVisibility.Public,
            Level = KnowledgeLevel.Beginner,
            CreatorId = Guid.NewGuid()
        };
    }
}


