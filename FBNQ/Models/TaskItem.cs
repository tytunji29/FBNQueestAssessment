namespace FBNQ.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relationships
        public Guid TeamId { get; set; }
        public Team Team { get; set; } = null!;

        public Guid AssignedToUserId { get; set; }
        public User AssignedTo { get; set; } = null!;

        public Guid CreatedByUserId { get; set; }
        public User CreatedBy { get; set; } = null!;
    }
}
