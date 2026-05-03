using Data;
using Data.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;
using TaskManagerItem = Domain.Task;

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
#pragma warning disable CS8625
        var task = new TaskManagerItem()
        {
            Id = 1,
            Title = null,
            Description = "Test description",
            DueDate = DateTime.Now,
            Status = TaskStatusEnum.Status.Pending
        };
#pragma warning restore CS8625

        Assert.Throws<AggregateException>(() => _taskRepository.AddTaskAsync(task).Result);
    }

    [Fact]
    public void AddTaskAsync_WhenTaskEntityIsValid_ShouldSucceed()
    {
        var task = new TaskManagerItem()
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

    [Fact]
    public void UpdateTaskAsync_WhenTaskEntityIsInvalid_ThrowsArgumentException()
    {
        var task = new TaskManagerItem()
        {
            Id = 1,
            Title = string.Empty,
            Description = "Test description",
            DueDate = DateTime.Now,
            Status = TaskStatusEnum.Status.Pending
        };

        Assert.ThrowsAsync<ArgumentException>(() => _taskRepository.UpdateTaskAsync(task));
    }

    [Fact]
    public async Task UpdateTaskAsync_WhenTaskEntityIsValid_ShouldSucceed()
    {
        var task = new TaskManagerItem()
        {
            Id = 1,
            Title = "Test Task",
            Description = "Test description",
            DueDate = DateTime.Now,
            Status = TaskStatusEnum.Status.Pending
        };
        await _taskRepository.AddTaskAsync(task);

        task.Title = "Updated Task";
        var result = await _taskRepository.UpdateTaskAsync(task);

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task UpdateTaskAsync_WhenTaskEntityNotExists_ShouldThrowException()
    {
        var task = new TaskManagerItem()
        {
            Id = 1,
            Title = "Test Task",
            Description = "Test description",
            DueDate = DateTime.Now,
            Status = TaskStatusEnum.Status.Pending
        };

        task.Title = "Updated Task";
        await Assert.ThrowsAnyAsync<DbUpdateConcurrencyException>(async () => await _taskRepository.UpdateTaskAsync(task));
    }

    [Fact]
    public async Task DeleteTaskAsync_WhenTaskEntityNotExists_ShouldReturnZeroLineAffected()
    {
        Assert.Equal(0, await _taskRepository.DeleteTaskAsync(1));
    }

    [Fact]
    public async Task DeleteTaskAsync_WhenTaskEntityExists_ShouldReturnOneLineAffected()
    {
        var task = new TaskManagerItem()
        {
            Id = 1,
            Title = "Test Task",
            Description = "Test description",
            DueDate = DateTime.Now,
            Status = TaskStatusEnum.Status.Pending
        };
        await _taskRepository.AddTaskAsync(task);

        Assert.Equal(1, await _taskRepository.DeleteTaskAsync(1));
    }

    [Fact]
    public async Task GetTaskByIdAsync_WhenTaskNotExists_ShouldReturnNull()
    {
        var result = await _taskRepository.GetTaskByIdAsync(1);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetTaskByIdAsync_WhenTaskExists_ShouldReturnTaskManagerItem()
    {
        var task = new TaskManagerItem()
        {
            Title = "Test Task",
            Description = "Test description",
            DueDate = DateTime.Now,
            Status = TaskStatusEnum.Status.Pending
        };
        await _taskRepository.AddTaskAsync(task);

        var result = await _taskRepository.GetTaskByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetAllTasksAsync_WhenNoTaskExists_ShouldReturnEmptyList()
    {
        var result = await _taskRepository.GetAllTasksAsync(null, null);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllTasksAsync_WhenTasksExist_ShouldReturnTaskList()
    {
        var task1 = new TaskManagerItem()
        {
            Title = "Test Task 1",
            Description = "Test description 1",
            DueDate = DateTime.Now,
            Status = TaskStatusEnum.Status.Pending
        };
        var task2 = new TaskManagerItem()
        {
            Title = "Test Task 2",
            Description = "Test description 2",
            DueDate = DateTime.Now,
            Status = TaskStatusEnum.Status.Completed
        };
        await _taskRepository.AddTaskAsync(task1);
        await _taskRepository.AddTaskAsync(task2);

        var result = await _taskRepository.GetAllTasksAsync(null, null);

        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllTasksAsync_FilterByStatusWhenTasksExist_ShouldReturnFilteredTaskList()
    {
        var task1 = new TaskManagerItem()
        {
            Title = "Test Task 1",
            Description = "Test description 1",
            DueDate = DateTime.Now,
            Status = TaskStatusEnum.Status.Pending
        };
        var task2 = new TaskManagerItem()
        {
            Title = "Test Task 2",
            Description = "Test description 2",
            DueDate = DateTime.Now,
            Status = TaskStatusEnum.Status.Completed
        };
        await _taskRepository.AddTaskAsync(task1);
        await _taskRepository.AddTaskAsync(task2);

        var result = await _taskRepository.GetAllTasksAsync(null, TaskStatusEnum.Status.Pending);

        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(TaskStatusEnum.Status.Pending, result.First().Status);
    }

    [Fact]
    public async Task GetAllTasksAsync_FilterByDueDateWhenTasksExist_ShouldReturnFilteredTaskList()
    {
        var task1 = new TaskManagerItem()
        {
            Title = "Test Task 1",
            Description = "Test description 1",
            DueDate = DateTime.Now,
            Status = TaskStatusEnum.Status.Pending
        };
        var task2 = new TaskManagerItem()
        {
            Title = "Test Task 2",
            Description = "Test description 2",
            DueDate = DateTime.Now,
            Status = TaskStatusEnum.Status.Completed
        };
        await _taskRepository.AddTaskAsync(task1);
        await _taskRepository.AddTaskAsync(task2);

        var result = await _taskRepository.GetAllTasksAsync(task1.DueDate, null);

        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(1, result.First().Id);
    }
}
