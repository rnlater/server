using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.SingleIdEntities;

namespace Domain.Entities.PivotEntities;

public class TrackSubject : PivotEntity
{
    public Guid TrackId { get; set; }
    [ForeignKey(nameof(TrackId))]
    public Track? Track { get; set; }

    public Guid SubjectId { get; set; }
    [ForeignKey(nameof(SubjectId))]
    public Subject? Subject { get; set; }
}
