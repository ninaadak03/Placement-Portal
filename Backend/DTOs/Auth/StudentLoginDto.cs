using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Auth;

public class StudentLoginDto
{
    [Required]
    [MinLength(11)]
    [MaxLength(12)]
    public string RollNo { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}