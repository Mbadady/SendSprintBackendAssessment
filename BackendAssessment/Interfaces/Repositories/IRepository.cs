
namespace BackendAssessment.Interfaces.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(int? skip = null, int? take = null, Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
    }
}
