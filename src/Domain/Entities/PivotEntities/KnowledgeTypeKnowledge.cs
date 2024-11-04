using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.SingleIdEntities;

namespace Domain.Entities.PivotEntities;

public class KnowledgeTypeKnowledge : PivotEntity
{
    public Guid KnowledgeTypeId { get; set; }
    [ForeignKey(nameof(KnowledgeTypeId))]
    public KnowledgeType? KnowledgeType { get; set; }

    public Guid KnowledgeId { get; set; }
    [ForeignKey(nameof(KnowledgeId))]
    public Knowledge? Knowledge { get; set; }
}
