using Backend.Data;
using Backend.DTOs.Application;
using Backend.DTOs.Auth;
using Backend.Interfaces;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class ApplicationService : IApplicationService
{
    private readonly ApplicationDbContext _context;

    public ApplicationService(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ApplicationResponseDto>> GetApplicationsForOpeningAsync(int openingId)
    {
        return await _context.Applications.Where(a => a.OpeningId == openingId)
            .Select(a => new ApplicationResponseDto
            {
                ApplicationId = a.Id,
                StudentId = a.StudentId,
                StudentName = a.Student.Name,
                RollNo = a.Student.RollNo,
                Branch = a.Student.Branch,
                CGPA = a.Student.CGPA,
                AppliedOn = a.AppliedOn,
                IsSelected = a.IsSelected
            }).ToListAsync();
    }

    public async Task<ServiceResponseDto> SelectStudentAsync(int applicationId)
    {
        // Get application
        Application? application = await _context.Applications.Include(a => a.Student).Include(a => a.Opening)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

        if (application == null)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Application not found."
            };
        }
        // Already selected
        if (application.IsSelected)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Student is already selected for this opening."
            };
        }

        Student student = application.Student;
        Opening opening = application.Opening;

        // Already placed
        if (student.IsPlaced)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Student is already placed."
            };
        }
        // Mark selected
        application.IsSelected = true;
        // Full-time offer
        if (opening.CTC.HasValue)
        {
            student.IsPlaced = true;
            student.PlacedCompanyId = opening.CompanyId;
            student.PlacedCTC = opening.CTC.Value;
        }
        await _context.SaveChangesAsync();
        return new ServiceResponseDto
        {
            Success = true,
            Message = "Student selected successfully."
        };
    }
}