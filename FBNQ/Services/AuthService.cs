using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FBNQ.Data;
using FBNQ.DTOs;
using FBNQ.Models;
using FBNQ.Services.Interfaces;
using BCrypt.Net;
using FBNQ.Repository;
using Azure.Core;

namespace FBNQ.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository _userRepo;
        private readonly IConfiguration _config;

        public AuthService(UserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<ReturnObject> RegisterAsync(RegisterRequest request)
        {
            var det = await _userRepo.RegisterAsync(request);
            if(det.res=="99")
                return new ReturnObject(false, "Email Already Exist", null);
            var token = GenerateJwtToken(det.user);
            return new ReturnObject(true, "User Registered Successfully", new { token, det.user.Email });
        }

        public async Task<ReturnObject> LoginAsync(LoginRequest request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return new ReturnObject(true, "Invalid credentials", null);

            var token = GenerateJwtToken(user);
            return new ReturnObject(true, "User Login Successfully", new { token, user.Email });
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _userRepo.GetByEmailAsync(email);
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
