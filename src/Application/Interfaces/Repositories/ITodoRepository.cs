using Application.DTOs;
using Application.Interfaces.SeedWorks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ITodoRepository : IRepository<Todo>
    {
        Task<bool> UpdateNameTodo(TodoInsert entity);
        Task<bool> UpdateActiveTodo(TodoInsert entity);
    }
}
