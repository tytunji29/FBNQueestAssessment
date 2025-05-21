using Azure.Core;
using FBNQ.Data;
using FBNQ.DTOs;
using FBNQ.Models;
using Microsoft.EntityFrameworkCore;

namespace FBNQ.Repository;


public class UserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<(string res, User? user)> RegisterAsync(RegisterRequest request)
    {
        if (await _db.Users.AnyAsync(u => u.Email == request.Email))
            return ("99", null);

        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "Superadmin"
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return ("00",user);
    }
    public  async Task<User?> GetByEmailAsync(string email)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
