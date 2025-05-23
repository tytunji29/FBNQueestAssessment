namespace FBNQ.Models
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
