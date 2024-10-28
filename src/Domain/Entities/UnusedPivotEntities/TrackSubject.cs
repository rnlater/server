using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.Entities.UnusedPivotEntities;

public class TrackSubject : UnusedPivotEntity
{
    public Guid TrackId { get; set; }
    [ForeignKey(nameof(TrackId))]
    public Track? Track { get; set; }

    public Guid SubjectId { get; set; }
    [ForeignKey(nameof(SubjectId))]
    public Subject? Subject { get; set; }
}
