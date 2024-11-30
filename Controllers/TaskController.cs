using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ToDoList.DTOs;
using ToDoList.Models;
using ToDoList.Repositories;
using Task = ToDoList.Models.Task; 

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TasksController(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        [HttpGet]
      
        [SwaggerResponse(200, "Success", typeof(List<TaskDto>))]
        public async Task<IActionResult> GetTasks(
            [FromQuery, SwaggerParameter("Filter tasks by completion status")] bool? completed,
            [FromQuery, SwaggerParameter("Filter tasks by due date")] DateTime? dueDate,
            [FromQuery, SwaggerParameter("Filter tasks by priority")] TaskPriority? priority)
        {
            var tasks = await _taskRepository.GetAllTasksAsync(completed, dueDate, priority);
            List<TaskDto> tasksDto = _mapper.Map<List<TaskDto>>(tasks);
            return Ok(tasksDto);
        }

        [HttpGet("{id}")]
     
        [SwaggerResponse(200, "Sucess", typeof(TaskDto))]
        [SwaggerResponse(404, "Task not found")]
        public async Task<IActionResult> GetTask(
            [SwaggerParameter("Unique identifier of the task")] int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null) return NotFound();

            return Ok(_mapper.Map<TaskDto>(task));
        }

        [HttpPost]
       
       
        [SwaggerResponse(201, "Task created successfully", typeof(TaskDto))]
        [SwaggerResponse(400, "Invalid task data")]
        public async Task<IActionResult> CreateTask(
            [FromBody, SwaggerRequestBody("Task creation details")] TaskDto createTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = _mapper.Map<Task>(createTaskDto);
            var createdTask = await _taskRepository.CreateTaskAsync(task);

            return CreatedAtAction(
                nameof(GetTask),
                new { id = createdTask.Id },
                _mapper.Map<TaskDto>(createdTask)
            );
        }

        [HttpPut("{id}")]
        
        [SwaggerResponse(200, "Task updated successfully", typeof(TaskDto))]
        [SwaggerResponse(400, "Invalid task data")]
        [SwaggerResponse(404, "Task not found")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody, SwaggerRequestBody("Task ")] TaskDto updateTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = _mapper.Map<Task>(updateTaskDto);
            task.Id = id;

            var updatedTask = await _taskRepository.UpdateTaskAsync(id, task);
            if (updatedTask == null) return NotFound();

            return Ok(_mapper.Map<TaskDto>(updatedTask));
        }

        [HttpDelete("{id}")]
        
        
        [SwaggerResponse(204, "Task deleted successfully")]
        [SwaggerResponse(404, "Task not found")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await _taskRepository.DeleteTaskAsync(id);
            return   NoContent();
        }
    }
}