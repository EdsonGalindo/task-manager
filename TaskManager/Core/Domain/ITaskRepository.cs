namespace Domain
{
    public interface ITaskRepository
    {
        Task<Task?> GetTaskByIdAsync(int id);
        Task<IEnumerable<Task>> GetAllTasksAsync(int? id, TaskStatusEnum.Status? status);
        Task<int> AddTaskAsync(Task task);
        Task<int> UpdateTaskAsync(Task task);
        Task<int> DeleteTaskAsync(int id);
    }
}
