using Backend.Data;
using Backend.DTOs.Student;
using Backend.DTOs.Admin;
using Backend.Interfaces;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _context;

    public AdminService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AdminStudentResponseDto>> GetStudentsAsync(string? branch, bool? isPlaced, int? companyId)
    {
        IQueryable<Student> query = _context.Students;

        // Branch filter
        if (!string.IsNullOrWhiteSpace(branch))
        {
            query = query.Where(s => s.Branch == branch);
        }

        // Placement filter
        if (isPlaced.HasValue)
        {
            query = query.Where(s => s.IsPlaced == isPlaced.Value);
        }

        // Company filter
        if (companyId.HasValue)
        {
            query = query.Where(s => s.PlacedCompanyId == companyId.Value);
        }

        return await query.Select(s => new AdminStudentResponseDto
        {
            StudentId = s.Id,
            Name = s.Name ?? string.Empty,
            RollNo = s.RollNo,
            Email = s.User.Email,
            Branch = s.Branch ?? string.Empty,
            CGPA = s.CGPA ?? 0m,
            IsPlaced = s.IsPlaced,
            PlacedCompanyName = s.PlacedCompany != null ? s.PlacedCompany.Name : null
        }).ToListAsync();
    }

    public async Task<AdminStudentDetailResponseDto?> GetStudentByIdAsync(int studentId)
    {
        return await _context.Students.Where(s => s.Id == studentId)
            .Select(s => new AdminStudentDetailResponseDto
            {
                StudentId = s.Id,
                Name = s.Name ?? string.Empty,
                RollNo = s.RollNo,
                Email = s.User.Email,
                PhoneNumber = s.PhoneNumber ?? string.Empty,
                Branch = s.Branch ?? string.Empty,
                Gender = s.Gender ?? string.Empty,
                DateOfBirth = s.DateOfBirth ?? DateOnly.MinValue,
                TenthPercentage = s.TenthPercentage ?? 0m,
                TwelfthPercentage = s.TwelfthPercentage ?? 0m,
                SgpaSem1 = s.SgpaSem1,
                SgpaSem2 = s.SgpaSem2,
                SgpaSem3 = s.SgpaSem3,
                SgpaSem4 = s.SgpaSem4,
                SgpaSem5 = s.SgpaSem5,
                SgpaSem6 = s.SgpaSem6,
                SgpaSem7 = s.SgpaSem7,
                SgpaSem8 = s.SgpaSem8,
                CGPA = s.CGPA ?? 0m,
                ResumeUrl = s.ResumeUrl ?? string.Empty,
                IsPlaced = s.IsPlaced,
                PlacedCompanyName = s.PlacedCompany != null ? s.PlacedCompany.Name : null,
                PlacedCTC = s.PlacedCTC
            }).FirstOrDefaultAsync();
    }

    public async Task<List<AdminStudentDetailResponseDto>> GetStudentDetailsAsync(string? branch, bool? isPlaced, int? companyId)
    {
        IQueryable<Student> query = _context.Students;

        // Branch filter
        if (!string.IsNullOrWhiteSpace(branch))
        {
            query = query.Where(s => s.Branch == branch);
        }

        // Placement filter
        if (isPlaced.HasValue)
        {
            query = query.Where(s => s.IsPlaced == isPlaced.Value);
        }

        // Company filter
        if (companyId.HasValue)
        {
            query = query.Where(s => s.PlacedCompanyId == companyId.Value);
        }

        return await query
            .Select(s => new AdminStudentDetailResponseDto
            {
                StudentId = s.Id,
                Name = s.Name ?? string.Empty,
                RollNo = s.RollNo,
                Email = s.User.Email,
                PhoneNumber = s.PhoneNumber ?? string.Empty,
                Branch = s.Branch ?? string.Empty,
                Gender = s.Gender ?? string.Empty,
                DateOfBirth = s.DateOfBirth ?? DateOnly.MinValue,
                TenthPercentage = s.TenthPercentage ?? 0m,
                TwelfthPercentage = s.TwelfthPercentage ?? 0m,
                SgpaSem1 = s.SgpaSem1,
                SgpaSem2 = s.SgpaSem2,
                SgpaSem3 = s.SgpaSem3,
                SgpaSem4 = s.SgpaSem4,
                SgpaSem5 = s.SgpaSem5,
                SgpaSem6 = s.SgpaSem6,
                SgpaSem7 = s.SgpaSem7,
                SgpaSem8 = s.SgpaSem8,
                CGPA = s.CGPA ?? 0m,
                ResumeUrl = s.ResumeUrl ?? string.Empty,
                IsPlaced = s.IsPlaced,
                PlacedCompanyName = s.PlacedCompany != null ? s.PlacedCompany.Name : null,
                PlacedCTC = s.PlacedCTC
            }).ToListAsync();
    }

    public async Task<AdminDashboardResponseDto> GetDashboardAsync()
    {
        int totalStudents = await _context.Students.CountAsync();
        int placedStudents = await _context.Students.CountAsync(s => s.IsPlaced);
        int totalCompanies = await _context.Companies.CountAsync();
        int totalOpenings = await _context.Openings.CountAsync();
        int totalApplications = await _context.Applications.CountAsync();
        decimal placementPercentage = 0;

        if (totalStudents > 0)
        {
            placementPercentage = Math.Round((decimal)placedStudents / totalStudents * 100, 2);
        }

        return new AdminDashboardResponseDto
        {
            TotalStudents = totalStudents,
            PlacedStudents = placedStudents,
            PlacementPercentage = placementPercentage,
            TotalCompanies = totalCompanies,
            TotalOpenings = totalOpenings,
            TotalApplications = totalApplications
        };
    }
}