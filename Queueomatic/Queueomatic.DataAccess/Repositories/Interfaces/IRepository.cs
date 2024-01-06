namespace Queueomatic.DataAccess.Repositories.Interfaces;

public interface IRepository<T, in TEntityType> where T : class
{
    Task<T?> GetAsync(TEntityType id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(TEntityType id);
}