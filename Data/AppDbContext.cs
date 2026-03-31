using Finals_Q1.Models;
using Microsoft.EntityFrameworkCore;

namespace Finals_Q1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TodoItem> Todos { get; set; }
}