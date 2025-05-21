using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FBNQ.DTOs;
using FBNQ.Services.Interfaces;

namespace FBNQ.Controllers
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
        /// <summary>
        /// Create A Team
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTeam(CreateTeamDto dto)
        {
            var result = await _teamService.CreateTeamAsync(GetUserId(), dto);
            return Ok(result);
        }
        /// <summary>
        /// Assign A User To a Team
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("{teamId}/users")]
        public async Task<IActionResult> AddUser(Guid teamId, AddUserToTeamDto dto)
        {
            var result  =await _teamService.AddUserToTeamAsync(GetUserId(), teamId, dto);
            return Ok(result);
        }
        /// <summary>
        /// Get All Teams
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AllTeams()
        {
            var teams = await _teamService.GetUserTeamsAsync();
            return Ok(teams);
        }
        /// <summary>
        /// Get All My teams Based On My token
        /// </summary>
        /// <returns></returns>
        [HttpGet("MyTeams")]
        public async Task<IActionResult> MyTeams()
        {
            var teams = await _teamService.GetUserTeamsAsync(GetUserId());
            return Ok(teams);
        }
    }
}
