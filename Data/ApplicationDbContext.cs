using Microsoft.EntityFrameworkCore;
using TeamTaskAPI.Models;

namespace TeamTaskAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<TeamUser> TeamUsers => Set<TeamUser>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TeamUser>()
                .HasKey(tu => new { tu.TeamId, tu.UserId });

            modelBuilder.Entity<TeamUser>()
                .HasOne(tu => tu.Team)
                .WithMany(t => t.TeamUsers)
                .HasForeignKey(tu => tu.TeamId);

            modelBuilder.Entity<TeamUser>()
                .HasOne(tu => tu.User)
                .WithMany(u => u.TeamUsers)
                .HasForeignKey(tu => tu.UserId);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Team)
                .WithMany(tn => tn.Tasks)
                .HasForeignKey(t => t.TeamId);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.AssignedTo)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.CreatedBy)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
