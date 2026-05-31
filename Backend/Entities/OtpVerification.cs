using System.ComponentModel.DataAnnotations;

namespace Backend.Entities;

public class OtpVerification
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(12)]
    public string RollNo { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(6)]
    public string OtpCode { get; set; } = string.Empty;

    [Required]
    public DateTime ExpiryTime { get; set; }

    public bool IsUsed { get; set; }
}