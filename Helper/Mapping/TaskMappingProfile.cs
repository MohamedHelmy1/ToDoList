using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using ToDoList.DTOs;


namespace ToDoList.Helper.Mapping
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<TaskDto, Task>().ReverseMap();

        }
    }
}
