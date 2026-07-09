using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Feedback;

public class CreateFeedbackDto
{
    [Required]
    public int CompanyId { get; set; }

    [Required]
    public int GraduationYear { get; set; }

    [Required]
    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;

    public decimal? CTC { get; set; }

    public decimal? Stipend { get; set; }

    [Required]
    [MaxLength(5000)]
    public string ProcessDescription { get; set; } = string.Empty;

    [Required]
    [MaxLength(3000)]
    public string Advice { get; set; } = string.Empty;
}