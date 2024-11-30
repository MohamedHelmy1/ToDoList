using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

        public DbSet<Task> Tasks { get; set; }


    }
}
