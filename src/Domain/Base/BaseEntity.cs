namespace Domain.Base
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; }

        protected BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}