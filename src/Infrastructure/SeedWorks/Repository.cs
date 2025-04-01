using Application.Interfaces.SeedWorks;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedWorks
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly TodoDbContext _context;
        private readonly DbSet<T>  _dbSet;

        public Repository(TodoDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                var rq = await _dbSet.AddAsync(entity);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var list = await _dbSet.ToListAsync();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if(entity == null)
                {
                    return false;
                }
                _dbSet.Remove(entity);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
