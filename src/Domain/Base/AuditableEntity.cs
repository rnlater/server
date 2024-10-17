namespace Domain.Base;
public abstract class AuditableEntity(Guid creator)
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; } = creator;
    public DateTime? ModifiedDate { get; set; }
    public Guid? ModifiedBy { get; set; }

    public void UpdateModificationInfo(Guid modifier)
    {
        ModifiedDate = DateTime.UtcNow;
        ModifiedBy = modifier;
    }
}
