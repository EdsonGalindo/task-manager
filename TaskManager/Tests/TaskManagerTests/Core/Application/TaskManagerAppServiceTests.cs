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
                Description = "Test description",
                DueDate = DateTime.Now,
                Status = TaskStatusEnum.Status.Pending
            };
            #pragma warning restore CS8625
            await Assert.ThrowsAsync<Exception>(async () => await _taskManagerAppService.CreateTaskAsync(taskDto));
        }
    }
}
