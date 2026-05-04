namespace Domain
{
    public interface ITaskRepository
    {
        Task<Task?> GetTaskByIdAsync(int id);
        Task<bool> GetTaskExistsByIdAsync(int id);
        Task<IEnumerable<Task>> GetAllTasksAsync(DateTime? id, TaskStatusEnum.Status? status);
        Task<Task> AddTaskAsync(Task task);
        Task<int> UpdateTaskAsync(Task task);
        Task<int> DeleteTaskAsync(int id);
    }
}
