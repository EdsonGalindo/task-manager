using Application.Dtos;
using Application.Services;
using Data;
using Data.Repositories;
using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;

namespace TaskManagerTests.Core.Application
{
    public class TaskManagerAppServiceTests
    {
        private readonly ITaskManagerAppService _taskManagerAppService;
        private readonly ITaskRepository _taskRepository;
        private readonly TaskContext _context;
        private readonly ILogger<TaskManagerAppService> _logger;

        #region Contants
        private const string TASK_TITLE = "Minha tarefa de teste";
        private const string TASK_DESCRIPTION = "Minha tarefa de teste";
        #endregion

        public TaskManagerAppServiceTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<TaskContext>()
                .UseSqlite(connection)
                .Options;
            _context = new TaskContext(options);
            _context.Database.EnsureCreated();
            _taskRepository = new TaskRepository(_context);
            _logger = new LoggerFactory().CreateLogger<TaskManagerAppService>();
            _taskManagerAppService = new TaskManagerAppService(_taskRepository, _logger);
        }

        [Fact]
        public async Task CreateTaskAsync_WhenTaskDtoIsInvalid_ThrowsException()
        {
#pragma warning disable CS8625
            var taskDto = new TaskDto()
            {
                Id = 1,
                Title = null,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now,
                Status = TaskStatusEnum.Status.Pending
            };
#pragma warning restore CS8625

            await Assert.ThrowsAsync<Exception>(async () => await _taskManagerAppService.CreateTaskAsync(taskDto));
        }

        [Fact]
        public async Task CreateTaskAsync_WhenTaskDtoIsValid_ShouldSucceed()
        {
            var taskDto = new TaskDto()
            {
                Id = 1,
                Title = TASK_TITLE,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now,
                Status = TaskStatusEnum.Status.Pending
            };

            var result = await _taskManagerAppService.CreateTaskAsync(taskDto);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(TASK_TITLE, result.Title);
            Assert.Equal(TASK_DESCRIPTION, result.Description);
        }

        [Fact]
        public async Task GetTaskByIdAsync_WhenTaskIdIsInvalid_ThrowsException()
        {
            await Assert.ThrowsAsync<Exception>(async () => await _taskManagerAppService.GetTaskByIdAsync(999));
        }

        [Fact]
        public async Task GetTaskByIdAsync_WhenTaskIdIsValid_ShouldSucceed()
        {
            var taskDto = new TaskDto()
            {
                Id = 1,
                Title = TASK_TITLE,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now,
                Status = TaskStatusEnum.Status.Pending
            };
            await _taskManagerAppService.CreateTaskAsync(taskDto);

            var result = await _taskManagerAppService.GetTaskByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(TASK_TITLE, result.Title);
            Assert.Equal(TASK_DESCRIPTION, result.Description);
        }

        [Fact]
        public async Task DeleteTaskAsync_WhenTaskIdIsInvalid_ShouldReturnFalse()
        {
            Assert.False(await _taskManagerAppService.DeleteTaskAsync(999));
        }

        [Fact]
        public async Task DeleteTaskAsync_WhenTaskIdIsValid_ShouldReturnTrue()
        {
            var taskDto = new TaskDto()
            {
                Id = 1,
                Title = TASK_TITLE,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now,
                Status = TaskStatusEnum.Status.Pending
            };
            await _taskManagerAppService.CreateTaskAsync(taskDto);

            Assert.True(await _taskManagerAppService.DeleteTaskAsync(1));
        }

        [Fact]
        public async Task GetAllTasksAsync_WhenNoTasksExist_ShouldReturnEmptyList()
        {
            var result = await _taskManagerAppService.GetAllTasksAsync(null, null);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllTasksAsync_WhenTasksExist_ShouldReturnTaskList()
        {
            var taskDto1 = new TaskDto()
            {
                Id = 1,
                Title = TASK_TITLE,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now,
                Status = TaskStatusEnum.Status.Pending
            };
            var taskDto2 = new TaskDto()
            {
                Id = 2,
                Title = TASK_TITLE,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now.AddDays(1),
                Status = TaskStatusEnum.Status.InProgress
            };
            await _taskManagerAppService.CreateTaskAsync(taskDto1);
            await _taskManagerAppService.CreateTaskAsync(taskDto2);

            var result = await _taskManagerAppService.GetAllTasksAsync(null, null);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllTasksAsync_WhenFilteringByDate_ShouldReturnFilteredTasks()
        {
            var taskDto1 = new TaskDto()
            {
                Id = 1,
                Title = TASK_TITLE,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now,
                Status = TaskStatusEnum.Status.Pending
            };
            var taskDto2 = new TaskDto()
            {
                Id = 2,
                Title = TASK_TITLE,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now.AddDays(1),
                Status = TaskStatusEnum.Status.InProgress
            };
            await _taskManagerAppService.CreateTaskAsync(taskDto1);
            await _taskManagerAppService.CreateTaskAsync(taskDto2);

            var result = await _taskManagerAppService.GetAllTasksAsync(taskDto1.DueDate, null);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public async Task GetAllTasksAsync_WhenFilteringByStatus_ShouldReturnFilteredTasks()
        {
            var taskDto1 = new TaskDto()
            {
                Id = 1,
                Title = TASK_TITLE,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now,
                Status = TaskStatusEnum.Status.Pending
            };
            var taskDto2 = new TaskDto()
            {
                Id = 2,
                Title = TASK_TITLE,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now.AddDays(1),
                Status = TaskStatusEnum.Status.InProgress
            };
            await _taskManagerAppService.CreateTaskAsync(taskDto1);
            await _taskManagerAppService.CreateTaskAsync(taskDto2);

            var result = await _taskManagerAppService.GetAllTasksAsync(null, TaskStatusEnum.Status.Pending);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public async Task GetAllTasksAsync_WhenFilteringByDateAndStatus_ShouldReturnFilteredTasks()
        {
            var taskDto1 = new TaskDto()
            {
                Id = 1,
                Title = TASK_TITLE,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now,
                Status = TaskStatusEnum.Status.Pending
            };
            var taskDto2 = new TaskDto()
            {
                Id = 2,
                Title = TASK_TITLE,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now.AddDays(1),
                Status = TaskStatusEnum.Status.InProgress
            };
            await _taskManagerAppService.CreateTaskAsync(taskDto1);
            await _taskManagerAppService.CreateTaskAsync(taskDto2);

            var result = await _taskManagerAppService.GetAllTasksAsync(
                taskDto1.DueDate, TaskStatusEnum.Status.Pending);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public async Task GetAllTasksAsync_WhenFilteringByDateAndStatusWithNoMatches_ShouldReturnEmptyList()
        {
            var taskDto1 = new TaskDto()
            {
                Id = 1,
                Title = TASK_TITLE,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now,
                Status = TaskStatusEnum.Status.Pending
            };
            var taskDto2 = new TaskDto()
            {
                Id = 2,
                Title = TASK_TITLE,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now.AddDays(1),
                Status = TaskStatusEnum.Status.InProgress
            };
            await _taskManagerAppService.CreateTaskAsync(taskDto1);
            await _taskManagerAppService.CreateTaskAsync(taskDto2);

            var result = await _taskManagerAppService.GetAllTasksAsync(
                DateTime.Now.AddDays(2), TaskStatusEnum.Status.Completed);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task UpdateTaskAsync_WhenTaskDtoIsInvalid_ThrowsException()
        {
            var taskDto = new TaskDto()
            {
                Id = 1,
                Title = TASK_TITLE,
                Description = TASK_DESCRIPTION,
                DueDate = DateTime.Now,
                Status = TaskStatusEnum.Status.Pending
            };
            await _taskManagerAppService.CreateTaskAsync(taskDto);
            _context.ChangeTracker.Clear();

#pragma warning disable CS8625
            taskDto.Title = null;
#pragma warning restore CS8625
            var result = await Assert.ThrowsAsync<Exception>(async () => await _taskManagerAppService.UpdateTaskAsync(taskDto));

            Assert.NotNull(result);
            Assert.Equal("Erro ao atualizar tarefa", result.Message);
        }
    }
}
