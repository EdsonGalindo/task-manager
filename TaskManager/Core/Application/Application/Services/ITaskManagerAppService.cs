using Application.Dtos;
using Domain;

namespace Application.Services
{
    public interface ITaskManagerAppService
    {
        Task<IEnumerable<TaskDto>> GetAllTasksAsync(DateTime? date, TaskStatusEnum.Status? taskStatus);
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task<TaskDto> CreateTaskAsync(TaskDto taskDto);
        Task<bool> UpdateTaskAsync(TaskDto taskDto);
        Task<bool> DeleteTaskAsync(int id);
    }
}
