using Domain;
using System.ComponentModel.DataAnnotations;
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

        public Task ToTask => new()
        {
            Id = this.Id,
            Title = this.Title,
            Description = this.Description,
            DueDate = this.DueDate,
            Status = this.Status
        };
    }
}
