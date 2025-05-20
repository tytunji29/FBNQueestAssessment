using System.ComponentModel.DataAnnotations;

namespace TeamTaskAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();
        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
        public ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();
    }
}
