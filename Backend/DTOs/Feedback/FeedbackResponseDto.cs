namespace Backend.DTOs.Feedback;

public class FeedbackResponseDto
{
    public int FeedbackId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public int GraduationYear { get; set; }

    public string Role { get; set; } = string.Empty;

    public decimal? CTC { get; set; }

    public decimal? Stipend { get; set; }

    public string ProcessDescription { get; set; } = string.Empty;

    public string Advice { get; set; } = string.Empty;
}