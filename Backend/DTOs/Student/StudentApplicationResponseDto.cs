namespace Backend.DTOs.Student;

public class StudentApplicationResponseDto
{
    public int ApplicationId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public DateOnly AppliedOn { get; set; }

    public bool IsSelected { get; set; }
}