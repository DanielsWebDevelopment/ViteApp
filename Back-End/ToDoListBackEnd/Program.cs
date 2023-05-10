using Microsoft.EntityFrameworkCore;
using ToDoListBackEnd.Context;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseInMemoryDatabase("ToDoList"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Serilog to write to a log file
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("logs/log.txt")
    .CreateLogger();

// Configure the logging factory
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(dispose: true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

