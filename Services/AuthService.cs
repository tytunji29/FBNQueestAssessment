using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeamTaskAPI.Data;
using TeamTaskAPI.DTOs;
using TeamTaskAPI.Models;
using TeamTaskAPI.Services.Interfaces;
using BCrypt.Net;

namespace TeamTaskAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _db.Users.AnyAsync(u => u.Email == request.Email))
                throw new Exception("Email already exists");

            var user = new User
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            return new AuthResponse(token, user.Email);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            var token = GenerateJwtToken(user);
            return new AuthResponse(token, user.Email);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user ?? throw new Exception("User not found");
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"]!)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
