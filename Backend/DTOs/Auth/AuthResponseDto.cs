//This is what backend sends after successful login.

namespace Backend.DTOs.Auth;

public class AuthResponseDto
{
    public bool Success {get; set;}

    public string? Message {get; set;} =  string.Empty;
    
    public string? Token { get; set; } = string.Empty;

    public string? Role { get; set; } = string.Empty;

    public bool IsProfileCompleted { get; set; }
}