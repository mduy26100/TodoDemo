using Application.DTOs;
using Application.Interfaces.SeedWorks;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TodoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateTodo(TodoInsertName todo)
        {
            try
            {
                var entity = _mapper.Map<Todo>(todo);

                var rq = await _unitOfWork.todoRepository.CreateAsync(entity);
                return await _unitOfWork.SaveChange() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<TodoDataRespon>> GetAllTodo()
        {
            try
            {
                var list = await _unitOfWork.todoRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<TodoDataRespon>>(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> RemoveTodo(int id)
        {
            try
            {
                var rq = await _unitOfWork.todoRepository.RemoveAsync(id);
                return await _unitOfWork.SaveChange() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateActiveTodo(TodoInsert todo)
        {
            try
            {
                var rq = await _unitOfWork.todoRepository.UpdateActiveTodo(todo);
                return await _unitOfWork.SaveChange() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateNameTodo(TodoInsert todo)
        {
            try
            {
                var rq = await _unitOfWork.todoRepository.UpdateNameTodo(todo);
                return await _unitOfWork.SaveChange() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
