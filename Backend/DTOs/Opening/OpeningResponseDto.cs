namespace Backend.DTOs.Opening;

public class OpeningResponseDto
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public decimal? Stipend { get; set; }

    public decimal? CTC { get; set; }

    public int? MaxParticipants { get; set; }

    public decimal MinCGPA { get; set; }

    public decimal MinTenthPercentage { get; set; }

    public decimal MinTwelfthPercentage { get; set; }

    public string? AllowedBranches { get; set; }

    public int? MaxAge { get; set; }

    public DateTime ApplicationDeadline { get; set; }
}