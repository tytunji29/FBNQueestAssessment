using Microsoft.EntityFrameworkCore;
using TeamTaskAPI.Data;
using TeamTaskAPI.DTOs;
using TeamTaskAPI.Models;
using TeamTaskAPI.Services.Interfaces;

namespace TeamTaskAPI.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _db;

        public TeamService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Team> CreateTeamAsync(Guid userId, CreateTeamDto dto)
        {
            var team = new Team { Name = dto.Name };
            var teamUser = new TeamUser { Team = team, UserId = userId, Role = "Admin" };

            _db.Teams.Add(team);
            _db.TeamUsers.Add(teamUser);
            await _db.SaveChangesAsync();
            return team;
        }

        public async Task AddUserToTeamAsync(Guid adminUserId, Guid teamId, AddUserToTeamDto dto)
        {
            var adminRelation = await _db.TeamUsers.FirstOrDefaultAsync(tu => tu.TeamId == teamId && tu.UserId == adminUserId);
            if (adminRelation == null || adminRelation.Role != "Admin")
                throw new UnauthorizedAccessException("Only team admins can add users");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email) ??
                       throw new Exception("User not found");

            if (await _db.TeamUsers.AnyAsync(tu => tu.TeamId == teamId && tu.UserId == user.Id))
                throw new Exception("User already in team");

            var newRelation = new TeamUser { TeamId = teamId, UserId = user.Id, Role = dto.Role };
            _db.TeamUsers.Add(newRelation);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Team>> GetUserTeamsAsync(Guid userId)
        {
            return await _db.Teams
                .Include(t => t.TeamUsers)
                .Where(t => t.TeamUsers.Any(tu => tu.UserId == userId))
                .ToListAsync();
        }
    }
}
