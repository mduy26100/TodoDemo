using Application.DTOs;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.SeedWorks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TodoRepository : Repository<Todo>, ITodoRepository
    {
        public TodoRepository(TodoDbContext context) : base(context)
        {
        }

        public async Task<bool> UpdateActiveTodo(TodoInsert entity)
        {
            try
            {
                var rq = await _context.Todos.FindAsync(entity.Id);

                if (rq == null)
                {
                    return false;
                }

                rq.IsCompleted = true;
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateNameTodo(TodoInsert entity)
        {
            try
            {
                var rq = await _context.Todos.FindAsync(entity.Id);

                if (rq == null)
                {
                    return false;
                }

                rq.Name = entity.Name;
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
