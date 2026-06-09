namespace Backend.DTOs.Application;

public class ApplicationResponseDto
{
    public int ApplicationId { get; set; }

    public int StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public string RollNo { get; set; } = string.Empty;

    public string Branch { get; set; } = string.Empty;

    public decimal CGPA { get; set; }

    public DateOnly AppliedOn { get; set; }

    public bool IsSelected { get; set; }
}