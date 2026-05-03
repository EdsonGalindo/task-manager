using Application.Dtos;
using Domain;
using Task = Domain.Task;

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

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _taskRepository.DeleteTaskAsync(id) > 0;
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync(DateTime? date, TaskStatusEnum.Status? taskStatus)
        {
            return await _taskRepository.GetAllTasksAsync(date, taskStatus) is IEnumerable<Task> tasks ?
                tasks.Select(TaskDto.Task2TaskDto.Compile()) :
                throw new Exception("Falha ao obter tarefas");
        }

        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetTaskByIdAsync(id) is Task task ?
                TaskDto.Task2TaskDto.Compile().Invoke(task) :
                throw new Exception("Tarefa não encontrada");
        }

        public async Task<bool> UpdateTaskAsync(TaskDto taskDto)
        {
            return await _taskRepository.UpdateTaskAsync(taskDto.ToTask) > 0;
        }
    }
}
