using Application.Dtos;
using Application.Services;
using Asp.Versioning;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TaskManager.WebApp.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskManagerAppService _taskManagerAppService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(
            ITaskManagerAppService taskManagerAppService,
            ILogger<TaskController> logger)
        {
            _taskManagerAppService = taskManagerAppService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Obtém uma lista de tarefas cadastradas no sistema", 
            Description = "obtém uma lista de tarefas cadastradas no sistema, podendo ser realizado filtro por data de vencimento ou status")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks(
            [FromQuery] DateTime? dueDate,
            [FromQuery] TaskStatusEnum.Status? status)
        {
            try
            {
                var result = await _taskManagerAppService.GetAllTasksAsync(dueDate, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "GetAllTasks - Ocorreu um erro ao obter as tarefas. Mensagem: {Message}",
                    ex.Message);
                return BadRequest("Ocorreu um erro ao obter as tarefas, tente novamente mais tarde.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Obtém uma tarefa cadastrada no sistema",
            Description = "Obtém uma tarefa previamente cadastrada no sistema")]
        public async Task<ActionResult<TaskDto>> GetTaskById(int id)
        {
            try
            {
                var result = await _taskManagerAppService.GetTaskByIdAsync(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "GetTaskById - Ocorreu um erro ao obter a tarefa com ID {Id}. Mensagem: {Message}",
                    id, ex.Message);
                return BadRequest("Ocorreu um erro ao obter a tarefa, tente novamente mais tarde.");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Cadastra uma nova tarefa no sistema",
            Description = "Cadastra uma nova tarefa no sistema de acordo com os dados informados")]
        public async Task<ActionResult<TaskDto>> CreateTask([FromBody] TaskDto taskDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _taskManagerAppService.CreateTaskAsync(taskDto);
                return CreatedAtAction(nameof(GetTaskById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "CreateTask - Ocorreu um erro ao criar a tarefa. Mensagem: {Message}",
                    ex.Message);
                return BadRequest("Ocorreu um erro ao criar a tarefa, tente novamente mais tarde.");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Atualiza uma tarefa existente no sistema",
            Description = "Atualiza as informações de uma tarefa previamente cadastrada no sistema")]
        public async Task<ActionResult> UpdateTask([FromBody] TaskDto taskDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var taskExists = await _taskManagerAppService.GetTaskExistsByIdAsync(taskDto.Id);
                if (!taskExists)
                {
                    return NotFound();
                }

                var result = await _taskManagerAppService.UpdateTaskAsync(taskDto);
                if (!result)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "UpdateTask - Ocorreu um erro ao atualizar a tarefa com ID {Id}. Mensagem: {Message}",
                    taskDto.Id,
                    ex.Message);
                return BadRequest("Ocorreu um erro ao atualizar a tarefa, tente novamente mais tarde.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Remove uma tarefa existente no sistema",
            Description = "Remove uma tarefa previamente cadastrada no sistema")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            try
            {
                var result = await _taskManagerAppService.DeleteTaskAsync(id);
                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "DeleteTask - Ocorreu um erro ao deletar a tarefa com ID {Id}. Mensagem: {Message}",
                    id,
                    ex.Message);
                return BadRequest("Ocorreu um erro ao deletar a tarefa, tente novamente mais tarde.");
            }
        }
    }
}
