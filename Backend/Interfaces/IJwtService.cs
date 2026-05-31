using Backend.Entities;
namespace Backend.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}