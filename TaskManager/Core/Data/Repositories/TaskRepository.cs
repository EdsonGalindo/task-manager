using Domain;
using Microsoft.EntityFrameworkCore;
using Task = Domain.Task;

namespace Data.Repositories
{
    public class TaskRepository(TaskContext context) : ITaskRepository
    {
        private readonly TaskContext _context = context;

        public async Task<Task> AddTaskAsync(Task task)
        {
            _context.Tasks.Add(task);
            await SaveDbChanges();
            return task;
        }

        public async Task<int> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                return await SaveDbChanges();
            }
            return default;
        }

        public async Task<IEnumerable<Task>> GetAllTasksAsync(DateTime? date, TaskStatusEnum.Status? status)
        {
            return await _context.Tasks
                .Where(t => (!date.HasValue || t.DueDate == date) && (!status.HasValue || t.Status == status))
                .ToListAsync();
        }

        public async Task<Task?> GetTaskByIdAsync(int id)
        {            
             return await _context.Tasks.FindAsync(id);
        }

        public async Task<int> UpdateTaskAsync(Task task)
        {
            _context.Tasks.Update(task);
            return await SaveDbChanges();
        }

        private async Task<int> SaveDbChanges()
        {
             return await _context.SaveChangesAsync();
        }
    }
}
