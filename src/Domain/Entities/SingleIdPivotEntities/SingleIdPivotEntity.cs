using System;
using Domain.Base;
using Microsoft.Extensions.Options;

namespace Domain.Entities.SingleIdPivotEntities;

public class SingleIdPivotEntity : BaseEntity
{
    public Guid Id { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }

    protected SingleIdPivotEntity() : base()
    {
        Id = Guid.NewGuid();
    }

    public void UpdateModificationInfo(Guid modifier)
    {
        ModifiedBy = modifier;
        ModifiedAt = DateTime.UtcNow;
    }
}
