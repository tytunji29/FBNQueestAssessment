using FBNQ.DTOs;
using FBNQ.Models;

namespace FBNQ.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ReturnObject> RegisterAsync(RegisterRequest request);
        Task<ReturnObject> LoginAsync(LoginRequest request);
        Task<User> GetByEmailAsync(string email);
    }
}
