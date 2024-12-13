using DistributedLogging.Application.Interfaces.Repositories;
using DistributedLogging.Presistence.Contexts;
using Microsoft.EntityFrameworkCore;


namespace DistributedLogging.Infrastructure.Presistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDBContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public IQueryable<T> GetAll()
        {
            return _context
                 .Set<T>()
                 .AsQueryable();
        }
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }
        public async Task UpdateAsync(T entity)
            => _context.Entry(entity).State = EntityState.Modified;
        public async Task DeleteAsync(T entity)
            => _context.Set<T>().Remove(entity);
        public async Task<bool> SaveChangesAsync()
        {
            int result = await _context.SaveChangesAsync();
            return result >= 0;
        }
        public async Task RemoveRangeAsync(List<T> entities)
            => _context.Set<T>().RemoveRange(entities);
        public void Attach(T entity)
            => _context.Set<T>().Attach(entity);
        public void AttachRange(List<T> entities)
            => _context.Set<T>().AttachRange(entities);
        public async Task AddRangeAsync(List<T> entities)
            => await _context.Set<T>().AddRangeAsync(entities);

        public async Task<T> GetByIdAsync(long id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task UpdateAsync(T entity, long id)
        {
            var local =_context.Set<T>().Find(id);
            if(local != null) _context.Entry(local).State = EntityState.Detached;   
            _context.Entry(entity).State = EntityState.Modified;

        }
    }
}
