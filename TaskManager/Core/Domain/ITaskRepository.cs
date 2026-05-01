namespace Domain
{
    public interface ITaskRepository
    {
        Task GetTaskById(int id);
        IEnumerable<Task> GetAllTasks(int? id, TaskStatusEnum.Status? status);
        void AddTask(Task task);
        void UpdateTask(Task task);
        void DeleteTask(int id);
    }
}
