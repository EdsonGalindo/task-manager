using Domain;
using NSwag.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Task = Domain.Task;

namespace Application.Dtos
{
    public class TaskDto
    {        
        public int Id { get; set; }

        [Required(ErrorMessage = "Título é obrigatório")]
        public required string Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public required TaskStatusEnum.Status Status { get; set; }

        [JsonIgnore]
        public Task ToTask => new()
        {
            Id = this.Id,
            Title = this.Title,
            Description = this.Description,
            DueDate = this.DueDate,
            Status = this.Status
        };

        public static Expression<Func<Task, TaskDto>> Task2TaskDto => task => new TaskDto()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status
        };
    }
}
