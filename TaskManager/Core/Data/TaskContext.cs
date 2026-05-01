using Microsoft.EntityFrameworkCore;
using Task = Domain.Task;

namespace Data
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

        protected TaskContext() { }

        public DbSet<Task> Tasks { get; set; }
    }
}
