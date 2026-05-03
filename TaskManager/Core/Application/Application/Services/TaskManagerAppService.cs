using Domain;

namespace Application.Services
{
    public class TaskManagerAppService : ITaskManagerAppService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskManagerAppService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public Task<TaskDto> CreateTaskAsync(TaskDto taskDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTaskAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskDto>> GetAllTasksAsync(DateTime? date, TaskStatusEnum.Status? taskStatus)
        {
            throw new NotImplementedException();
        }

        public Task<TaskDto> GetTaskByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTaskAsync(TaskDto taskDto)
        {
            throw new NotImplementedException();
        }
    }
}
