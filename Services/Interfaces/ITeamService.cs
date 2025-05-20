using TeamTaskAPI.DTOs;
using TeamTaskAPI.Models;

namespace TeamTaskAPI.Services.Interfaces
{
    public interface ITeamService
    {
        Task<Team> CreateTeamAsync(Guid userId, CreateTeamDto dto);
        Task AddUserToTeamAsync(Guid adminUserId, Guid teamId, AddUserToTeamDto dto);
        Task<IEnumerable<Team>> GetUserTeamsAsync(Guid userId);
    }
}
