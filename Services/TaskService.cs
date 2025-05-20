using Microsoft.EntityFrameworkCore;
using TeamTaskAPI.Data;
using TeamTaskAPI.DTOs;
using TeamTaskAPI.Models;
using TeamTaskAPI.Services.Interfaces;

namespace TeamTaskAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _db;

        public TaskService(ApplicationDbContext db)
        {
            _db = db;
        }

        private async Task<bool> IsUserInTeam(Guid userId, Guid teamId)
        {
            return await _db.TeamUsers.AnyAsync(tu => tu.UserId == userId && tu.TeamId == teamId);
        }

        public async Task<IEnumerable<TaskItem>> GetTasksAsync(Guid userId, Guid teamId)
        {
            if (!await IsUserInTeam(userId, teamId)) throw new UnauthorizedAccessException();

            return await _db.Tasks
                .Where(t => t.TeamId == teamId)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task<TaskItem> CreateTaskAsync(Guid userId, Guid teamId, CreateTaskDto dto)
        {
            if (!await IsUserInTeam(userId, teamId)) throw new UnauthorizedAccessException();

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                TeamId = teamId,
                AssignedToUserId = dto.AssignedToUserId,
                CreatedByUserId = userId
            };
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto dto)
        {
            var task = await _db.Tasks.FindAsync(taskId) ?? throw new Exception("Task not found");
            if (!await IsUserInTeam(userId, task.TeamId)) throw new UnauthorizedAccessException();

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.DueDate = dto.DueDate;
            task.AssignedToUserId = dto.AssignedToUserId;
            await _db.SaveChangesAsync();
            return task;
        }

        public async Task DeleteTaskAsync(Guid userId, Guid taskId)
        {
            var task = await _db.Tasks.FindAsync(taskId) ?? throw new Exception("Task not found");
            if (!await IsUserInTeam(userId, task.TeamId)) throw new UnauthorizedAccessException();

            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
        }

        public async Task<TaskItem> UpdateTaskStatusAsync(Guid userId, Guid taskId, UpdateTaskStatusDto dto)
        {
            var task = await _db.Tasks.FindAsync(taskId) ?? throw new Exception("Task not found");
            if (!await IsUserInTeam(userId, task.TeamId)) throw new UnauthorizedAccessException();

            task.Status = dto.Status;
            await _db.SaveChangesAsync();
            return task;
        }
    }
}
