using Xunit;
using TeamTaskAPI.Services;
using TeamTaskAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;
using TeamTaskAPI.DTOs;

namespace TeamTaskAPI.Tests
{
    public class TeamServiceTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly TeamService _teamService;

        public TeamServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new ApplicationDbContext(options);
            _teamService = new TeamService(_dbContext);
        }

        [Fact]
        public async void CanCreateTeam()
        {
            var userId = Guid.NewGuid();
            _dbContext.Users.Add(new Models.User { Id = userId, Email = "test@test.com", PasswordHash = "hash" });
            await _dbContext.SaveChangesAsync();

            var team = await _teamService.CreateTeamAsync(userId, new CreateTeamDto("My Team"));
            Assert.Equal("My Team", team.Name);
            Assert.Single(team.TeamUsers);
        }
    }
}
