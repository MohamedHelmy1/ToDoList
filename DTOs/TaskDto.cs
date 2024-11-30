using System.ComponentModel.DataAnnotations;
using ToDoList.Models;

namespace ToDoList.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }

       
        public string Title { get; set; }

      
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

    
        public bool IsCompleted { get; set; } = false;

      
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
