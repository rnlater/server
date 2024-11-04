using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.PivotEntities;

namespace Domain.Entities.SingleIdEntities;

public class KnowledgeTopic : SingleIdEntity
{
    public required string Title { get; set; }

    public int? Order { get; set; }

    public Guid? ParentId { get; set; }

    [ForeignKey("ParentId")]
    public KnowledgeTopic? Parent { get; set; }

    [InverseProperty("Parent")]
    public ICollection<KnowledgeTopic> Children { get; set; } = [];

    public ICollection<KnowledgeTopicKnowledge> KnowledgeTopicKnowledges { get; set; } = [];

    public static KnowledgeTopic ForTestPurposeOnly()
    {
        return new KnowledgeTopic
        {
            Id = Guid.NewGuid(),
            Title = "Test KnowledgeTopic",
        };
    }
}
