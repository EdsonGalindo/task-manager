using Microsoft.EntityFrameworkCore;
using Task = Domain.Task;

namespace Data
{
    public class TaskContext : DbContext
    {
        public required DbSet<Task> Tasks { get; set; }
    }
}
