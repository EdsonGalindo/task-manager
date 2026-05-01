using System.ComponentModel.DataAnnotations;

namespace Domain
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
    }
}
