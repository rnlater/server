using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities.SingleIdEntities;

public class PublicationRequest : SingleIdEntity
{
    public Guid KnowledgeId { get; set; }
    [ForeignKey(nameof(KnowledgeId))]
    public Knowledge? Knowledge { get; set; }

    public PublicationRequestStatus Status { get; set; } = PublicationRequestStatus.Pending;
}
