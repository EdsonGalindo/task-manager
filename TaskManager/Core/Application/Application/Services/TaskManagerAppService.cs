using Application.Dtos;
using Domain;

namespace Application.Services
{
    public class TaskManagerAppService(ITaskRepository taskRepository) : ITaskManagerAppService
    {
        private readonly ITaskRepository _taskRepository = taskRepository;

        public async Task<TaskDto> CreateTaskAsync(TaskDto taskDto)
        {
            return await _taskRepository.AddTaskAsync(taskDto.ToTask) > 0 ? 
                taskDto :
                throw new Exception("Falha ao criar tarefa");
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
