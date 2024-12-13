namespace DistributedLogging.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(long id);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(List<T> entities);
        Task UpdateAsync(T entity, long id);
        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
        Task RemoveRangeAsync(List<T> entity);
        Task<bool> SaveChangesAsync();
        void AttachRange(List<T> entities);
        void Attach(T entity);
    }
}
