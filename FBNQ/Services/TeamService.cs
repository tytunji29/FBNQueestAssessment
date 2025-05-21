using Microsoft.EntityFrameworkCore;
using FBNQ.Data;
using FBNQ.DTOs;
using FBNQ.Models;
using FBNQ.Services.Interfaces;
using FBNQ.Repository;

namespace FBNQ.Services
{
    public class TeamService : ITeamService
    {
        private readonly TeamRepository _teamRepo;
        private readonly UserRepository _userRepo;
        public TeamService(TeamRepository teamRepo, UserRepository userRepo)
        {
            _teamRepo = teamRepo;
            _userRepo = userRepo;
        }

        public async Task<ReturnObject> CreateTeamAsync(Guid userId, CreateTeamDto dto)
        {
            _teamRepo.CreateTeam(userId, dto);
            return new ReturnObject(true, "Team Added Successfully", null);
        }

        public async Task<ReturnObject> AddUserToTeamAsync(Guid adminUserId, Guid teamId, AddUserToTeamDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null)
                return new ReturnObject(false, "User not found", null);


            if (await _teamRepo.Det(teamId,user.Id))
                return new ReturnObject(false, "User already in team", null);

           _teamRepo.CreateUserToTeam(adminUserId, teamId, dto,user);
            
            return new ReturnObject(true, "User Added To A Team Successfully", null);
        }

        public async Task<ReturnObject> GetUserTeamsAsync(Guid userId)
        {
            var rec = await _teamRepo.GetMyTeam(userId);
            return new ReturnObject(true, "Team Successfully", rec);
        }
        public async Task<ReturnObject> GetUserTeamsAsync()
        {
            var rec = await _teamRepo.GetAllTeam();
            return new ReturnObject(true, "Team Successfully", rec);
        }
    }
}
