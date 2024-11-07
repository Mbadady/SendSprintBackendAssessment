namespace BackendAssessment.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IAppDbContext _context;

        public Repository(IAppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(int? skip = null, int? take = null, Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }


        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Set<T>().FindAsync([id], cancellationToken);

            return entity ?? throw new ResourceNotFoundException($"Entity with ID {id} not found.");
        }

        public async Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
