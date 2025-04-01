using Application.Interfaces.Repositories;

namespace Application.Interfaces.SeedWorks
{
    public interface IUnitOfWork : IDisposable
    {
        ITodoRepository todoRepository { get; }
        Task<int> SaveChange();
    }
}
