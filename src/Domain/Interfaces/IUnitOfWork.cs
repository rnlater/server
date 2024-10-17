using Domain.Base;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IRepository<T> Repository<T>() where T : BaseEntity;
        Task RollBackChangesAsync();
    }
}
