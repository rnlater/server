

using Domain.Base;

namespace Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T?> Delete(int id);
        Task<T?> Find(ISpecification<T> spec);
        Task<IEnumerable<T>> FindAll(ISpecification<T> spec);
        Task<int> Count(ISpecification<T> spec);
    }
}
