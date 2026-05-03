using Application.Dtos;
using Domain;
using Task = Domain.Task;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    /// <summary>
    /// Application service para gerenciamento de tarefas, implementando a interface ITaskManagerAppService e 
    /// utilizando o repositório ITaskRepository para realizar operações de CRUD em tarefas.
    /// </summary>
    /// <param name="taskRepository">O repositório de tarefas a ser utilizado pelo serviço.</param>
    public class TaskManagerAppService(
        ITaskRepository taskRepository,
        ILogger<TaskManagerAppService> logger) : ITaskManagerAppService
    {
        private readonly ITaskRepository _taskRepository = taskRepository;
        private readonly ILogger<TaskManagerAppService> _logger = logger;

        /// <inheritdoc/>
        public async Task<TaskDto> CreateTaskAsync(TaskDto taskDto)
        {
            try
            {
                    return await _taskRepository.AddTaskAsync(taskDto.ToTask) is Task task ?
                        TaskDto.Task2TaskDto.Compile().Invoke(task) :
                        throw new Exception("Falha ao criar tarefa");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "CreateTaskAsync - Erro ao criar tarefa. Mensagem: {errorMessage}",
                    ex.Message);
                throw new Exception("Erro ao criar tarefa");
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteTaskAsync(int id)
        {
            try
            {
                return await _taskRepository.DeleteTaskAsync(id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "DeleteTaskAsync - Erro ao deletar tarefa. Id: {taskId}, Mensagem: {errorMessage}",
                    id, ex.Message);
                throw new Exception("Erro ao deletar tarefa");
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync(DateTime? date, TaskStatusEnum.Status? taskStatus)
        {
            return await _taskRepository.GetAllTasksAsync(date, taskStatus) is IEnumerable<Task> tasks ?
                tasks.Select(TaskDto.Task2TaskDto.Compile()) :
                throw new Exception("Falha ao obter tarefas");
        }

        /// <inheritdoc/>
        public async Task<TaskDto?> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetTaskByIdAsync(id) is Task task ?
                TaskDto.Task2TaskDto.Compile().Invoke(task) :
                null;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateTaskAsync(TaskDto taskDto)
        {
            try
            {
                return await _taskRepository.UpdateTaskAsync(taskDto.ToTask) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "UpdateTaskAsync - Erro ao atualizar tarefa. Id: {taskId}, Mensagem: {errorMessage}",
                    taskDto.Id, ex.Message);
                throw new Exception("Erro ao atualizar tarefa");
            }
        }
    }
}
