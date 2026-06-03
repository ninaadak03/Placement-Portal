using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Opening;

public class CreateOpeningDto
{
    [Required]
    public int CompanyId { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(150)]
    public string Role { get; set; } = string.Empty;

    [Range(0, 100000)]
    public decimal? Stipend { get; set; }

    [Range(0, 10000000)]
    public decimal? CTC { get; set; }

    [Range(1, int.MaxValue)]
    public int? MaxParticipants { get; set; }

    [Range(0, 10)]
    public decimal MinCGPA { get; set; }

    [Range(0, 100)]
    public decimal MinTenthPercentage { get; set; }

    [Range(0, 100)]
    public decimal MinTwelfthPercentage { get; set; }

    public string? AllowedBranches { get; set; }

    [Range(0, 100)]
    public int? MaxAge { get; set; }

    [Required]
    public DateTime ApplicationDeadline { get; set; }
}