using Domain.Base;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Save changes to the database
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Get the repository for the entity T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepository<T> Repository<T>() where T : BaseEntity;

        /// <summary>
        /// Roll back changes to the database
        /// </summary>
        /// <returns></returns>
        Task RollBackChangesAsync();
    }
}
