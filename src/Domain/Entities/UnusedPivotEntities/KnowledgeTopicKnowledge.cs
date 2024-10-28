using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.Entities.UnusedPivotEntities;

public class KnowledgeTopicKnowledge : UnusedPivotEntity
{
    public Guid KnowledgeTopicId { get; set; }
    [ForeignKey(nameof(KnowledgeTopicId))]
    public KnowledgeTopic? KnowledgeTopic { get; set; }

    public Guid KnowledgeId { get; set; }
    [ForeignKey(nameof(KnowledgeId))]
    public Knowledge? Knowledge { get; set; }
}
