using AdminLibrary.DTOs;
using AdminLibrary.Models;

namespace AdminLibrary.Services.Interfaces;

public interface IAuthService
{
    User ValidateUser(string email, string password);
    void Add(RegisterDto dto);
}