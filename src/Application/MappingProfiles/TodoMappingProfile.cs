using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class TodoMappingProfile : Profile
    {
        public TodoMappingProfile()
        {
            CreateMap<TodoInsert, Todo>();
            CreateMap<TodoInsertName, Todo>();
            CreateMap<TodoInsertActive, Todo>();
            CreateMap<Todo, TodoDataRespon>();
        }
    }
}
