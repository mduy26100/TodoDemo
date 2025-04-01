namespace Application.Interfaces.SeedWorks
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> CreateAsync(T entity);
        Task<bool> RemoveAsync(int id);
    }
}
