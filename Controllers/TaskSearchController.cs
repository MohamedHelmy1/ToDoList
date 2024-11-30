using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ToDoList.DTOs;
using ToDoList.Repositories;
using ToDoList.Models;
using Task = ToDoList.Models.Task;
using AutoMapper;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TaskStatusController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TaskStatusController(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        [HttpPut("{id}/complete")]
        
        [SwaggerResponse(200, "Task  completed ", typeof(TaskDto))]
        [SwaggerResponse(404, "Task not found")]
        public async Task<ActionResult<TaskDto>> MarkTaskAsCompleted( int id)
        {
            var task = await _taskRepository.UpdateTaskStatusAsync(id, true);
            if (task == null) return NotFound();
            return Ok(_mapper.Map<TaskDto>(task));
        }

        [HttpPut("{id}/incomplete")]
      
        [SwaggerResponse(200, "Task  incompleted ", typeof(TaskDto))]
        [SwaggerResponse(404, "Task not found")]
        public async Task<ActionResult<TaskDto>> MarkTaskAsIncomplete(int id)
        {
            var task = await _taskRepository.UpdateTaskStatusAsync(id, false);
            if (task == null) return NotFound();
            return Ok(_mapper.Map<TaskDto>(task));
        }

        [HttpPut("{id}/priority")]
      
        public async Task<ActionResult<TaskDto>> UpdateTaskPriority(int id,
            [FromQuery, SwaggerParameter("UpdateTaskPriority")] TaskPriority priority)
        {
            var task = await _taskRepository.UpdateTaskPriorityAsync(id, priority);
            if (task == null) return NotFound();
            return Ok(_mapper.Map<TaskDto>(task));
        }
    }
}