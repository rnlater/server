namespace Domain.Entities.SingleIdEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Enums;

public class Knowledge : SingleIdEntity
{
    public required string Title { get; set; }
    public KnowledgeVisibility Visibility { get; set; }
    public KnowledgeLevel Level { get; set; }
    public Guid CreatorId { get; set; }

    [ForeignKey("CreatorId")]
    public User? Creator { get; set; }

    public ICollection<Material> Materials { get; set; } = [];
    public ICollection<SubjectKnowledge> SubjectKnowledges { get; set; } = [];
    public ICollection<KnowledgeTypeKnowledge> KnowledgeTypeKnowledges { get; set; } = [];
    public ICollection<KnowledgeTopicKnowledge> KnowledgeTopicKnowledges { get; set; } = [];
    public ICollection<Learning> Learnings { get; set; } = [];
    public ICollection<GameKnowledgeSubscription> GameKnowledgeSubscriptions { get; set; } = [];

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


