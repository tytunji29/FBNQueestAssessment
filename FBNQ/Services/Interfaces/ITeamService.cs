using FBNQ.DTOs;
using FBNQ.Models;

namespace FBNQ.Services.Interfaces
{
    public interface ITeamService
    {
        Task<ReturnObject> CreateTeamAsync(Guid userId, CreateTeamDto dto);
        Task<ReturnObject> AddUserToTeamAsync(Guid adminUserId, Guid teamId, AddUserToTeamDto dto);
        Task<ReturnObject> GetUserTeamsAsync(Guid userId); 
        Task<ReturnObject> GetUserTeamsAsync();
    }
}
