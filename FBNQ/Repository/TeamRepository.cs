using FBNQ.Data;
using FBNQ.DTOs;
using FBNQ.Models;
using Microsoft.EntityFrameworkCore;

namespace FBNQ.Repository;
public class TeamRepository
{
    private readonly ApplicationDbContext _db;

    public TeamRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<List<TeamDto>> GetMyTeam(Guid userId)
    {
        var rec = await _db.Teams
.Include(t => t.TeamUsers)
    .ThenInclude(tu => tu.User).Where(t => t.TeamUsers.Any(tu => tu.UserId == userId))
.Include(t => t.Tasks)
.Select(t => new TeamDto(
    t.Id,
    t.Name,
    t.TeamUsers.Select(tu => new TeamUserDto(
        tu.UserId.ToString(),
        tu.User.Email
    )).ToList(),
    t.Tasks.Select(task => new TeamTaskDto(
        task.Id.ToString(),
        task.Title,
        task.Description
    )).ToList()
))
.ToListAsync();
        return rec;
    }
    public async Task<List<TeamDto>> GetAllTeam()
    {
        var rec = await _db.Teams
 .Include(t => t.TeamUsers)
     .ThenInclude(tu => tu.User)
 .Include(t => t.Tasks)
 .Select(t => new TeamDto(
     t.Id,
     t.Name,
     t.TeamUsers.Select(tu => new TeamUserDto(
         tu.UserId.ToString(),
         tu.User.Email
     )).ToList(),
     t.Tasks.Select(task => new TeamTaskDto(
         task.Id.ToString(),
         task.Title,
         task.Description
     )).ToList()
 ))
 .ToListAsync();
        return rec;
    }

    public async Task<bool> Det(Guid teamId, Guid userId)
    {
        return await _db.TeamUsers.AnyAsync(tu => tu.TeamId == teamId && tu.UserId == userId);
    }
    public async void CreateUserToTeam(Guid adminUserId, Guid teamId, AddUserToTeamDto dto, User user)
    {
        var newRelation = new TeamUser { TeamId = teamId, UserId = user.Id, Role = dto.Role, CreatedBy = adminUserId.ToString(), CreatedAt = DateTime.UtcNow };
        _db.TeamUsers.Add(newRelation);
        await _db.SaveChangesAsync();
    }
    public async void CreateTeam(Guid userId, CreateTeamDto dto)
    {
        var team = new Team
        {
            Name = dto.Name,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "Superadmin"
        };

        _db.Teams.Add(team);
        await _db.SaveChangesAsync();
    }
}
