using AdminLibrary.Data;
using AdminLibrary.DTOs;
using AdminLibrary.Services.Interfaces;
using AdminLibrary.Models;

namespace AdminLibrary.Services;


public class AuthService :IAuthService
{
    private readonly  AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public User ValidateUser(string email, string password )
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Email == email && u.Password == password);

        return user;
    }

    public void Add(RegisterDto dto)
    {
        var user = new User
        {   
            Username =  dto.Username,
            Email = dto.Email,
            Password = dto.Password
        };
        _context.Users.Add(user);
        _context.SaveChanges();
    }
    
    
}