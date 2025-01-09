

using Domain.Base;

namespace Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Add an entity to the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> Add(T entity);

        /// <summary>
        /// Update an entity in the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> Update(T entity);

        /// <summary>
        /// Delete an entity from the repository by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T?> Delete(Guid id);

        /// <summary>
        /// Delete an entity from the repository by entity (for a pivot table)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T?> Delete(T entity);

        /// <summary>
        /// Get an entity from the repository by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T?> GetById(Guid id);

        /// <summary>
        /// Find an entity in the repository by specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<T?> Find(ISpecification<T> spec);

        /// <summary>
        /// Get all entities from the repository
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAll();

        /// <summary>
        /// Find many entities in the repository by specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindMany(ISpecification<T> spec);

        /// <summary>
        /// Count entities in the repository by specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<int> Count(ISpecification<T> spec);
        Task<int> Count();
    }
}
