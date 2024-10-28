using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.Entities.UnusedPivotEntities;

public class KnowledgeTypeKnowledge : UnusedPivotEntity
{
    public Guid KnowledgeTypeId { get; set; }
    [ForeignKey(nameof(KnowledgeTypeId))]
    public KnowledgeType? KnowledgeType { get; set; }

    public Guid KnowledgeId { get; set; }
    [ForeignKey(nameof(KnowledgeId))]
    public Knowledge? Knowledge { get; set; }
}
