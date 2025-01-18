using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Entities.Models;

namespace Todo.Infrastructure.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Todo.db");
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<TodoGroup> TodoGroups { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<RequestLog> RequestLogs { get; set; }
    }
}