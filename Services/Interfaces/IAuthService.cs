using TeamTaskAPI.DTOs;
using TeamTaskAPI.Models;

namespace TeamTaskAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<User> GetByEmailAsync(string email);
    }
}
