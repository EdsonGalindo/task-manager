using Application.Dtos;
using Application.Services;
using Data;
using Data.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace TaskManagerTests.Core.Application
{
    public class TaskManagerAppServiceTests
    {
        private readonly ITaskManagerAppService _taskManagerAppService;
        private readonly ITaskRepository _taskRepository;
        private readonly TaskContext _context;

        public TaskManagerAppServiceTests()
        {
            var options = new DbContextOptionsBuilder<TaskContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new TaskContext(options);
            _taskRepository = new TaskRepository(_context);
            _taskManagerAppService = new TaskManagerAppService(_taskRepository);
        }

        [Fact]
        public async void CreateTaskAsync_WhenTaskDtoIsInvalid_ThrowsException()
        {
            var taskDto = new TaskDto()
            {
                Id = 1,
                Title = string.Empty,
                Description = "Test description",
                DueDate = DateTime.Now,
                Status = TaskStatusEnum.Status.Pending
            };
            await Assert.ThrowsAsync<Exception>(() => _taskManagerAppService.CreateTaskAsync(taskDto));
        }
    }
}
