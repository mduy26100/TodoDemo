using Application.Interfaces.Repositories;
using Application.Interfaces.SeedWorks;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        public ITodoRepository todoRepository { get; }

        private readonly TodoDbContext _context;

        public UnitOfWork(TodoDbContext context)
        {
            _context = context;
            todoRepository = new TodoRepository(context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
