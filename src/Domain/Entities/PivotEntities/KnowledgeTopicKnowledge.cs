using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.SingleIdEntities;

namespace Domain.Entities.PivotEntities;

public class KnowledgeTopicKnowledge : PivotEntity
{
    public Guid KnowledgeTopicId { get; set; }
    [ForeignKey(nameof(KnowledgeTopicId))]
    public KnowledgeTopic? KnowledgeTopic { get; set; }

    public Guid KnowledgeId { get; set; }
    [ForeignKey(nameof(KnowledgeId))]
    public Knowledge? Knowledge { get; set; }
}
