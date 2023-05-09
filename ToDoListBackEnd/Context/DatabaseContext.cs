using Microsoft.EntityFrameworkCore;
using ToDoListBackEnd.Models;

namespace ToDoListBackEnd.Context;
public class DatabaseContext : DbContext
{
	public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<ToDoItem> ToDoItems { get; set; } = null!;
}

