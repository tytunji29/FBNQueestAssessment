using Microsoft.AspNetCore.Mvc;
using FBNQ.DTOs;
using FBNQ.Services.Interfaces;

namespace FBNQ.Controllers
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
        /// <summary>
        /// To Register A User
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }
        /// <summary>
        /// To Login User
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }
        /// <summary>
        /// To Get Your Details From Token
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        [HttpGet("me")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult Me()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            return Ok(new { Email = email });
        }
    }
}
