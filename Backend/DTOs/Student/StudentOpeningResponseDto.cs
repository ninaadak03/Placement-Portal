namespace Backend.DTOs.Student;

public class StudentOpeningResponseDto
{
    public int OpeningId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public decimal? Stipend { get; set; }

    public decimal? CTC { get; set; }

    public decimal MinCGPA { get; set; }

    public decimal MinTenthPercentage { get; set; }

    public decimal MinTwelfthPercentage { get; set; }

    public string? AllowedBranches { get; set; }

    public int? MaxAge { get; set; }

    public DateTime ApplicationDeadline { get; set; }

    public bool IsEligible { get; set; }

    public bool HasApplied { get; set; }
}