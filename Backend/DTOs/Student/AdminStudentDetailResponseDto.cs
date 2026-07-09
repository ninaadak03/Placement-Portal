namespace Backend.DTOs.Student;

public class AdminStudentDetailResponseDto
{
    public int StudentId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string RollNo { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Branch { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }

    public decimal TenthPercentage { get; set; }

    public decimal TwelfthPercentage { get; set; }

    public decimal? SgpaSem1 { get; set; }

    public decimal? SgpaSem2 { get; set; }

    public decimal? SgpaSem3 { get; set; }

    public decimal? SgpaSem4 { get; set; }

    public decimal? SgpaSem5 { get; set; }

    public decimal? SgpaSem6 { get; set; }

    public decimal? SgpaSem7 { get; set; }

    public decimal? SgpaSem8 { get; set; }

    public decimal CGPA { get; set; }

    public string ResumeUrl { get; set; } = string.Empty;

    public bool IsPlaced { get; set; }

    public string? PlacedCompanyName { get; set; }

    public decimal? PlacedCTC { get; set; }
}