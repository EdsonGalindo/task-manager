using Application.Dtos;
using Domain;

namespace Application.Services
{
    /// <summary>
    /// Interface para o serviço de gerenciamento de tarefas, definindo os métodos para criar, ler, atualizar e excluir tarefas.
    /// </summary>
    public interface ITaskManagerAppService
    {
        /// <summary>
        /// Obtém todas as tarefas, podendo ser filtradas por data de vencimento e/ou status. 
        /// Se nenhum filtro for fornecido, retorna todas as tarefas.
        /// </summary>
        /// <param name="date">Data de vencimento para filtrar as tarefas.</param>
        /// <param name="taskStatus">Status da tarefa para filtrar as tarefas.</param>
        /// <returns>Uma coleção de TaskDto que atende aos critérios de filtro.</returns>
        Task<IEnumerable<TaskDto>> GetAllTasksAsync(DateTime? date, TaskStatusEnum.Status? taskStatus);

        /// <summary>
        /// Obtém uma tarefa específica pelo seu ID. Se a tarefa não for encontrada, lança uma exceção.
        /// </summary>
        /// <param name="id">ID da tarefa a ser obtida.</param>
        /// <returns>O TaskDto correspondente ao ID fornecido.</returns>
        Task<TaskDto> GetTaskByIdAsync(int id);

        /// <summary>
        /// Cria uma nova tarefa com base no TaskDto fornecido. Se a criação for bem-sucedida, retorna o TaskDto criado; caso contrário, lança uma exceção.
        /// </summary>
        /// <param name="taskDto">Um objeto TaskDto contendo os dados da tarefa a ser criada.</param>
        /// <returns>O TaskDto criado.</returns>
        Task<TaskDto> CreateTaskAsync(TaskDto taskDto);

        /// <summary>
        /// Atualiza uma tarefa existente com base no TaskDto fornecido. Retorna true se a atualização for bem-sucedida; caso contrário, retorna false.
        /// </summary>
        /// <param name="taskDto">Um objeto TaskDto contendo os dados da tarefa a ser atualizada.</param>
        /// <returns>True se a atualização for bem-sucedida; caso contrário, false. </returns>
        Task<bool> UpdateTaskAsync(TaskDto taskDto);

        /// <summary>
        /// Remove uma tarefa específica pelo seu ID. Retorna true se a exclusão for bem-sucedida; caso contrário, retorna false.
        /// </summary>
        /// <param name="id">ID da tarefa a ser removida.</param>
        /// <returns>True se a exclusão for bem-sucedida; caso contrário, false.</returns>
        Task<bool> DeleteTaskAsync(int id);
    }
}
