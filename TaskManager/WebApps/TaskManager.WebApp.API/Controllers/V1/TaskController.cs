using Application.Dtos;
using Application.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.WebApp.API.Controllers.V1
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult> UpdateTask([FromBody] TaskDto taskDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _taskManagerAppService.UpdateTaskAsync(taskDto);
                if (!result)
                {
                    return NotFound();
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
