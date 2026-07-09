namespace Backend.DTOs.Student;

public class AdminStudentResponseDto
{
    public int StudentId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string RollNo { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Branch { get; set; } = string.Empty;

    public decimal CGPA { get; set; }

    public bool IsPlaced { get; set; }

    public string? PlacedCompanyName { get; set; }
}