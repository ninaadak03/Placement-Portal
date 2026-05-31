using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Auth;

public class StudentRegisterDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Roll No is required.")]
    [MinLength(11)]
    [MaxLength(12)]
    [RegularExpression(
    @"^(01|02)NC(22|23)(CS|IS|EC|EE|EI|ME|CV|BT)(?!000)\d{3}$",
    ErrorMessage = "Invalid Roll Number format."
    )]
    public string RollNo { get; set; } = string.Empty;

    [Required]
    [MinLength(8, ErrorMessage ="Password must be at least 8 characters long.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).+$",
        ErrorMessage = "Password must contain uppercase, lowercase, number, and special character."
    )]
    public string Password { get; set; } = string.Empty;
}