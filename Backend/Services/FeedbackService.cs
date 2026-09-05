using Backend.Data;
using Backend.DTOs.Auth;
using Backend.DTOs.Feedback;
using Backend.Entities;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class FeedbackService : IFeedbackService
{
    private readonly ApplicationDbContext _context;

    public FeedbackService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<ServiceResponseDto> CreateFeedbackAsync(int userId, CreateFeedbackDto dto)
    {
        Student? student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Student not found."
            };
        }

        bool companyExists = await _context.Companies.AnyAsync(c => c.Id == dto.CompanyId);

        if (!companyExists)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Company not found."
            };
        }

        // Trim text fields
        dto.Role = dto.Role.Trim();
        dto.ProcessDescription = dto.ProcessDescription.Trim();
        dto.Advice = dto.Advice.Trim();

        if (string.IsNullOrWhiteSpace(dto.Role))
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Role is required."
            };
        }

        if (string.IsNullOrWhiteSpace(dto.ProcessDescription))
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Process description is required."
            };
        }

        if (string.IsNullOrWhiteSpace(dto.Advice))
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Advice is required."
            };
        }

        bool selectedForCompany = await _context.Applications
                .AnyAsync(a => a.StudentId == student.Id && a.IsSelected && a.Opening.CompanyId == dto.CompanyId);

        if (!selectedForCompany)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "You can only post feedback for companies where you were selected."
            };
        }

        Feedback feedback = new()
        {
            StudentId = student.Id,
            CompanyId = dto.CompanyId,
            GraduationYear = dto.GraduationYear,
            Role = dto.Role,
            CTC = dto.CTC,
            Stipend = dto.Stipend,
            ProcessDescription = dto.ProcessDescription,
            Advice = dto.Advice
        };

        _context.Feedbacks.Add(feedback);

        await _context.SaveChangesAsync();

        return new ServiceResponseDto
        {
            Success = true,
            Message = "Feedback submitted successfully."
        };
    }

    public async Task<List<FeedbackResponseDto>> GetFeedbackAsync(string? studentName, string? companyName)
    {
        IQueryable<Feedback> query = _context.Feedbacks;

        if (!string.IsNullOrWhiteSpace(studentName))
        {
            query = query.Where(f => f.Student.Name != null && f.Student.Name.Contains(studentName));
        }

        if (!string.IsNullOrWhiteSpace(companyName))
        {
            query = query.Where(f => f.Company.Name.Contains(companyName));
        }

        return await query.OrderByDescending(f => f.Id)
            .Select(f => new FeedbackResponseDto
            {
                FeedbackId = f.Id,
                StudentName = f.Student.Name ?? string.Empty,
                CompanyName = f.Company.Name,
                GraduationYear = f.GraduationYear,
                Role = f.Role,
                CTC = f.CTC,
                Stipend = f.Stipend, 
                ProcessDescription = f.ProcessDescription,
                Advice = f.Advice
            }).ToListAsync();
    }

    public async Task<ServiceResponseDto> DeleteFeedbackAsync(int userId, string role, int feedbackId)
    {
        Feedback? feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.Id == feedbackId);

        if (feedback == null)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Feedback not found."
            };
        }

        // Admin can delete anything
        if (role == "Admin")
        {
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            return new ServiceResponseDto
            {
                Success = true,
                Message = "Feedback deleted successfully."
            };
        }

        // Student can delete only own feedback
        Student? student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Student not found."
            };
        }

        if (feedback.StudentId != student.Id)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "You can delete only your own feedback."
            };
        }
        _context.Feedbacks.Remove(feedback);
        await _context.SaveChangesAsync();

        return new ServiceResponseDto
        {
            Success = true,
            Message = "Feedback deleted successfully."
        };
    }    
}