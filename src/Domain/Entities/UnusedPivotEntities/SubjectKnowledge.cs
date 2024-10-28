using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.Entities.UnusedPivotEntities;

public class SubjectKnowledge : UnusedPivotEntity
{
    public Guid SubjectId { get; set; }
    [ForeignKey(nameof(SubjectId))]
    public Subject? Subject { get; set; }

    public Guid KnowledgeId { get; set; }
    [ForeignKey(nameof(KnowledgeId))]
    public Knowledge? Knowledge { get; set; }
}
