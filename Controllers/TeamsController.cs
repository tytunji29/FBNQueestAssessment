using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamTaskAPI.DTOs;
using TeamTaskAPI.Services.Interfaces;

namespace TeamTaskAPI.Controllers
{
    [ApiController]
    [Route("teams")]
    [Authorize]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpPost]
        public async Task<IActionResult> CreateTeam(CreateTeamDto dto)
        {
            var result = await _teamService.CreateTeamAsync(GetUserId(), dto);
            return Ok(result);
        }

        [HttpPost("{teamId}/users")]
        public async Task<IActionResult> AddUser(Guid teamId, AddUserToTeamDto dto)
        {
            await _teamService.AddUserToTeamAsync(GetUserId(), teamId, dto);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> MyTeams()
        {
            var teams = await _teamService.GetUserTeamsAsync(GetUserId());
            return Ok(teams);
        }
    }
}
