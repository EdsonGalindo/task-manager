using Data;
using Data.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using Task = Domain.Task;

namespace TaskManagerTests.Core.Data;

public class TaskRepositoryTests
{
    private readonly ITaskRepository _taskRepository;
    private readonly TaskContext _context;

    public TaskRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TaskContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TaskContext(options);
        _taskRepository = new TaskRepository(_context);
    }

    [Fact]
    public void AddTaskAsync_WhenTaskEntityIsInvalid_ThrowsArgumentException()
    {
        var task = new Task()
        {
            Id = 1,
            Title = string.Empty,
            Description = "Test description",
            DueDate = DateTime.Now,
            Status = TaskStatusEnum.Status.Pending
        };

        Assert.ThrowsAsync<ArgumentException>(() => _taskRepository.AddTaskAsync(task));
    }

    [Fact]
    public void AddTaskAsync_WhenTaskEntityIsValid_ShouldSucceed()
    {
        var task = new Task()
        {
            Id = 1,
            Title = "Test Task",
            Description = "Test description",
            DueDate = DateTime.Now,
            Status = TaskStatusEnum.Status.Pending
        };
        
        var result = _taskRepository.AddTaskAsync(task);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }
}
