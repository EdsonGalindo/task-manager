using Application.Dtos;
using Domain;
using Task = Domain.Task;

namespace Application.Services
{
    /// <summary>
    /// Application service para gerenciamento de tarefas, implementando a interface ITaskManagerAppService e 
    /// utilizando o repositório ITaskRepository para realizar operações de CRUD em tarefas.
    /// </summary>
    /// <param name="taskRepository">O repositório de tarefas a ser utilizado pelo serviço.</param>
    public class TaskManagerAppService(ITaskRepository taskRepository) : ITaskManagerAppService
    {
        private readonly ITaskRepository _taskRepository = taskRepository;

        /// <inheritdoc/>
        public async Task<TaskDto> CreateTaskAsync(TaskDto taskDto)
        {
            return await _taskRepository.AddTaskAsync(taskDto.ToTask) > 0 ? 
                taskDto :
                throw new Exception("Falha ao criar tarefa");
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _taskRepository.DeleteTaskAsync(id) > 0;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync(DateTime? date, TaskStatusEnum.Status? taskStatus)
        {
            return await _taskRepository.GetAllTasksAsync(date, taskStatus) is IEnumerable<Task> tasks ?
                tasks.Select(TaskDto.Task2TaskDto.Compile()) :
                throw new Exception("Falha ao obter tarefas");
        }

        /// <inheritdoc/>
        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetTaskByIdAsync(id) is Task task ?
                TaskDto.Task2TaskDto.Compile().Invoke(task) :
                throw new Exception("Tarefa não encontrada");
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateTaskAsync(TaskDto taskDto)
        {
            return await _taskRepository.UpdateTaskAsync(taskDto.ToTask) > 0;
        }
    }
}
