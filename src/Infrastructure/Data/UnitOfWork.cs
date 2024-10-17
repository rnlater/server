using Domain.Base;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Vocab.Infrastructure.Data
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context = context;

        private readonly IDictionary<Type, dynamic> _listOfRepositories = new Dictionary<Type, dynamic>();

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            if (_listOfRepositories.ContainsKey(typeof(T)))
            {
                return _listOfRepositories[typeof(T)];
            }

            var repositoryType = typeof(Repository<>);

            var repository = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context) ?? throw new InvalidOperationException("Repository is null");

            _listOfRepositories.Add(typeof(T), repository);

            return (IRepository<T>)repository;
        }

        public async Task RollBackChangesAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}