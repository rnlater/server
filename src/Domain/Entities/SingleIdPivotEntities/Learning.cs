using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.SingleIdEntities;

namespace Domain.Entities.SingleIdPivotEntities;

public class Learning : SingleIdPivotEntity
{
    public Learning() : base() { }

    public Guid KnowledgeId { get; set; }
    [ForeignKey(nameof(KnowledgeId))]
    public Knowledge? Knowledge { get; set; }

    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public DateTime NextReviewDate { get; set; }

    public ICollection<LearningHistory> LearningHistories { get; set; } = [];

}
