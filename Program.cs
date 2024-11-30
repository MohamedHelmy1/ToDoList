using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ToDoList.Models;
using ToDoList.Repositories;
using AutoMapper;
using ToDoList.Helper.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();


builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefeletCon")));


builder.Services.AddScoped<ITaskRepository, TaskRepository>();


builder.Services.AddAutoMapper(typeof(TaskMappingProfile)); 


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tasks API", Version = "v1" });
    c.EnableAnnotations();
});
var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo List API v1"));

app.UseHttpsRedirection();


app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
