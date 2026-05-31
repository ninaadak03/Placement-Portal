namespace Backend.DTOs.Student;

using System.ComponentModel.DataAnnotations;

public class CompleteProfileDto
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Phone]
    [RegularExpression(@"^[6-9]\d{9}$",
    ErrorMessage = "Enter a valid 10-digit Indian mobile number.")]
    [MaxLength(10)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Branch { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Gender { get; set; } = string.Empty;

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal TenthPercentage { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal TwelfthPercentage { get; set; }

    [Range(0, 10)]
    public decimal? SgpaSem1 { get; set; }
    
    [Range(0, 10)]
    public decimal? SgpaSem2 { get; set; }

    [Range(0, 10)]
    public decimal? SgpaSem3 { get; set; }

    [Range(0, 10)]
    public decimal? SgpaSem4 { get; set; }

    [Range(0, 10)]
    public decimal? SgpaSem5 { get; set; }

    [Range(0, 10)]
    public decimal? SgpaSem6 { get; set; }

    [Range(0, 10)]
    public decimal? SgpaSem7 { get; set; }

    [Range(0, 10)]
    public decimal? SgpaSem8 { get; set; }

    [Required]
    [Range(0, 10)]
    public decimal CGPA { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(500)]
    public string ResumeUrl { get; set; } = string.Empty;
}