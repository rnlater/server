using Domain.Base;

namespace Domain.Entities.AuditablePivotEntities;

public class AuditablePivotEntity : BaseEntity
{
    public Guid? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public Guid? ModifiedBy { get; set; }

    protected AuditablePivotEntity(Guid creator) : base()
    {
        CreatedBy = creator;
    }

    protected AuditablePivotEntity() : base()
    {
        CreatedBy = null;
    }

    public void UpdateModificationInfo(Guid modifier)
    {
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = modifier;
    }
}
