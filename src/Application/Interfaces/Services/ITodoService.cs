using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoDataRespon>> GetAllTodo();
        Task<bool> CreateTodo(TodoInsertName todo);
        Task<bool> UpdateNameTodo(TodoInsert todo);
        Task<bool> UpdateActiveTodo(TodoInsert todo);
        Task<bool> RemoveTodo(int id);
    }
}
