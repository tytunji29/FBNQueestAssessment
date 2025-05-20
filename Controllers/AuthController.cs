using Microsoft.AspNetCore.Mvc;
using TeamTaskAPI.DTOs;
using TeamTaskAPI.Services.Interfaces;

namespace TeamTaskAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        [HttpGet("me")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult Me()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            return Ok(new { Email = email });
        }
    }
}
