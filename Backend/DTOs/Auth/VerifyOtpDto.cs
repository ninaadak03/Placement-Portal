using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Auth;

public class VerifyOtpDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "OTP is required.")] 
    [MaxLength(10)]
    public string OtpCode { get; set; } = string.Empty;

    [Required]
    [MinLength(11)]
    [MaxLength(12)]
    public string RollNo { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}