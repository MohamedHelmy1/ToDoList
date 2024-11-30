using Microsoft.EntityFrameworkCore;
using Task = ToDoList.Models.Task; // Use alias for your custom Task model
using System.Threading.Tasks; // Keep this for async methods
using ToDoList.Models;
using ToDoList.Repositories;

namespace ToDoList.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TodoDbContext _context;

        public TaskRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Task>> GetAllTasksAsync(bool? completed = null, DateTime? dueDate = null, TaskPriority? priority = null)
        {
            var query = _context.Tasks.AsQueryable();

            if (completed.HasValue)
                query = query.Where(t => t.IsCompleted == completed.Value);

            if (dueDate.HasValue)
                query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == dueDate.Value.Date);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            return await query.ToListAsync();
        }

        public async Task<Task?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<Task> CreateTaskAsync(Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<Task?> UpdateTaskAsync(int id, Task task)
        {
            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null) return null;

            existingTask.Title = task.Title ?? existingTask.Title;
            existingTask.Description = task.Description ?? existingTask.Description;
            existingTask.DueDate = task.DueDate ?? existingTask.DueDate;
            existingTask.Priority = task.Priority;
            existingTask.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingTask;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Task?> UpdateTaskStatusAsync(int id, bool isCompleted)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return null;

            task.IsCompleted = isCompleted;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<Task?> UpdateTaskPriorityAsync(int id, TaskPriority priority)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return null;

            task.Priority = priority;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return task;
        }
    }
}