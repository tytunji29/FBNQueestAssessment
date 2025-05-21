using Microsoft.EntityFrameworkCore;
using FBNQ.Data;
using FBNQ.DTOs;
using FBNQ.Models;
using FBNQ.Services.Interfaces;

namespace FBNQ.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _db;

        public TeamService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ReturnObject> CreateTeamAsync(Guid userId, CreateTeamDto dto)
        {
            var team = new Team
            {
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "Superadmin"
            };

            _db.Teams.Add(team);
            await _db.SaveChangesAsync();
            return new ReturnObject(true, "Team Added Successfully", null);
        }

        public async Task<ReturnObject> AddUserToTeamAsync(Guid adminUserId, Guid teamId, AddUserToTeamDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return new ReturnObject(false, "User not found", null);


            if (await _db.TeamUsers.AnyAsync(tu => tu.TeamId == teamId && tu.UserId == user.Id))
                return new ReturnObject(false, "User already in team", null);

            var newRelation = new TeamUser { TeamId = teamId, UserId = user.Id, Role = dto.Role, CreatedBy = adminUserId.ToString(), CreatedAt = DateTime.UtcNow };
            _db.TeamUsers.Add(newRelation);
            await _db.SaveChangesAsync();
            return new ReturnObject(true, "User Added To A Team Successfully", null);
        }

        public async Task<ReturnObject> GetUserTeamsAsync(Guid userId)
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

            return new ReturnObject(true, "Team Successfully", rec);
        }
        public async Task<ReturnObject> GetUserTeamsAsync()
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


            return new ReturnObject(true, "Team Successfully", rec);
        }
    }
}
