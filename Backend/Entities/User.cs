using System.ComponentModel.DataAnnotations;

namespace Backend.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Role { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? RollNo { get; set; }

    public bool IsVerified { get; set; }
}