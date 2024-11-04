using Domain.Base;

namespace Domain.Entities.SingleIdEntities;

public class SingleIdEntity : BaseEntity
{
    public Guid Id { get; set; }
    protected SingleIdEntity() : base()
    {
        Id = Guid.NewGuid();
    }
}
