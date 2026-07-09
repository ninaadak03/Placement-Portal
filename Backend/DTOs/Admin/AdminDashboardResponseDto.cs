namespace Backend.DTOs.Admin;

public class AdminDashboardResponseDto
{
    public int TotalStudents { get; set; }

    public int PlacedStudents { get; set; }

    public decimal PlacementPercentage { get; set; }

    public int TotalCompanies { get; set; }

    public int TotalOpenings { get; set; }

    public int TotalApplications { get; set; }
}