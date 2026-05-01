using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public required TaskStatusEnum.Status Status { get; set; }
    }
}
