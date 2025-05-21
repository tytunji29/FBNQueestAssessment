using FBNQ.Data;
using FBNQ.DTOs;
using FBNQ.Models;
using Microsoft.EntityFrameworkCore;

namespace FBNQ.Repository;

public class TaskRepository
{
    private readonly ApplicationDbContext _db;

    public TaskRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    private async Task<bool> IsUserInTeam(Guid userId, Guid teamId)
    {
        return await _db.TeamUsers.AnyAsync(tu => tu.UserId == userId && tu.TeamId == teamId);
    }
    private async Task<bool> IsUserAnAdmin(Guid userId, Guid teamId)
    {
        return await _db.TeamUsers.AnyAsync(tu => tu.UserId == userId && tu.TeamId == teamId && tu.Role == "Admin");
    }
    public async Task<List<TaskDto>> GetTask(Guid userId, Guid teamId)
    {
        if (!await IsUserInTeam(userId, teamId)) throw new UnauthorizedAccessException();
        var rec = await _db.Tasks
 .Where(t => t.TeamId == teamId)
 .Include(t => t.Team)
 .Include(t => t.AssignedTo)
 .Select(t => new TaskDto(
     t.Id.ToString(),
     t.Title, t.Description, t.DueDate, (Models.TaskStatus)t.Status, t.CreatedAt, t.Team != null ? t.Team.Name : null,
     t.AssignedTo != null ? t.AssignedTo.Email : null
 ))
 .ToListAsync();
        return rec;
    }
    public async void AddTask(Guid userId, Guid teamId, CreateTaskDto dto)
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
    }
    public async void UpdateTask(Guid userId, Guid taskId, UpdateTaskDto dto)
    {
        var task = await _db.Tasks.FindAsync(taskId) ?? throw new Exception("Task not found");
        if (!await IsUserInTeam(userId, task.TeamId)) throw new UnauthorizedAccessException();
        if (!await IsUserAnAdmin(userId, task.TeamId)) throw new Exception("Only The Team Admin Can Update A Task");

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.DueDate = dto.DueDate;
        task.AssignedToUserId = dto.AssignedToUserId;
        await _db.SaveChangesAsync();
    }
    public async void UpdateTaskStatus(Guid userId, Guid taskId, UpdateTaskStatusDto dto)
    {
        var task = await _db.Tasks.FindAsync(taskId) ?? throw new Exception("Task not found");
        if (!await IsUserInTeam(userId, task.TeamId)) throw new UnauthorizedAccessException();
        if (!await IsUserAnAdmin(userId, task.TeamId)) throw new Exception("Only The Team Admin Can Update A Task Status");

        task.Status = dto.Status;
        await _db.SaveChangesAsync();
    }   
    public async void DeleteTask(Guid userId, Guid taskId)
    {
        var task = await _db.Tasks.FindAsync(taskId) ?? throw new Exception("Task not found");
        if (!await IsUserInTeam(userId, task.TeamId)) throw new UnauthorizedAccessException();
        if (!await IsUserAnAdmin(userId, task.TeamId)) throw new Exception("Only The Team Admin Can Delete A Task");

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
    }
}
