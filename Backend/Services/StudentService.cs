using Backend.Data;
using Backend.Entities;
using Backend.DTOs.Auth;
using Backend.DTOs.Student;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;

    public StudentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentProfileResponseDto> GetProfileAsync(int userId)
    {
        Student? student = await _context.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            throw new Exception("Student not found.");
        }

        return new StudentProfileResponseDto
        {
            Email = student.User.Email,
            RollNo = student.RollNo,

            Name = student.Name ?? string.Empty,
            PhoneNumber = student.PhoneNumber ?? string.Empty,
            Branch = student.Branch ?? string.Empty,
            Gender = student.Gender ?? string.Empty,
            DateOfBirth = student.DateOfBirth ?? DateOnly.MinValue,

            TenthPercentage = student.TenthPercentage ?? 0m,
            TwelfthPercentage = student.TwelfthPercentage ?? 0m,

            SgpaSem1 = student.SgpaSem1,
            SgpaSem2 = student.SgpaSem2,
            SgpaSem3 = student.SgpaSem3,
            SgpaSem4 = student.SgpaSem4,
            SgpaSem5 = student.SgpaSem5,
            SgpaSem6 = student.SgpaSem6,
            SgpaSem7 = student.SgpaSem7,
            SgpaSem8 = student.SgpaSem8,

            CGPA = student.CGPA ?? 0m,

            ResumeUrl = student.ResumeUrl ?? string.Empty,

            IsProfileCompleted = student.IsProfileCompleted
        };
    }

    public async Task<ServiceResponseDto> CompleteProfileAsync(int userId,CompleteProfileDto dto)
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

        dto.Name = (dto.Name ?? string.Empty).Trim();
        dto.ResumeUrl = (dto.ResumeUrl ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return new ServiceResponseDto { Success = false, Message = "Full Name is required." };
        }

        if (string.IsNullOrWhiteSpace(dto.ResumeUrl))
        {
            return new ServiceResponseDto { Success = false, Message = "Resume URL is required." };
        }

        if (student.IsProfileCompleted)
        {
            if (student.Branch != dto.Branch || student.Gender != dto.Gender || student.DateOfBirth != dto.DateOfBirth)
            {
                return new ServiceResponseDto
                {
                    Success = false,
                    Message = "Branch, Gender and Date of Birth cannot be modified after profile completion."
                };
            }
        }
        if (!ValidBranches.Contains(dto.Branch))
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Invalid branch."
            };
        }

        student.Name = dto.Name;
        student.PhoneNumber = dto.PhoneNumber;
        student.TenthPercentage = dto.TenthPercentage;
        student.TwelfthPercentage = dto.TwelfthPercentage;
        student.SgpaSem1 = dto.SgpaSem1;
        student.SgpaSem2 = dto.SgpaSem2;
        student.SgpaSem3 = dto.SgpaSem3;
        student.SgpaSem4 = dto.SgpaSem4;
        student.SgpaSem5 = dto.SgpaSem5;
        student.SgpaSem6 = dto.SgpaSem6;
        student.SgpaSem7 = dto.SgpaSem7;
        student.SgpaSem8 = dto.SgpaSem8;
        student.CGPA = dto.CGPA;
        student.ResumeUrl = dto.ResumeUrl;

        if (!student.IsProfileCompleted)
        {
            student.Branch = dto.Branch;
            student.Gender = dto.Gender;
            student.DateOfBirth = dto.DateOfBirth;
            student.IsProfileCompleted = true;
        }

        await _context.SaveChangesAsync();
        return new ServiceResponseDto
        {
            Success = true,
            Message = "Profile saved successfully."
        };
    }

    public async Task<List<StudentOpeningResponseDto>> GetAvailableOpeningsAsync(int userId)
    {
        Student? student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);

        if(student == null)
        {
            throw new Exception("Student not found.");
        }
        
        var openings = await _context.Openings.Include(o => o.Company)
            .Where(o => o.ApplicationDeadline > DateTime.UtcNow)
            .ToListAsync();
        
        List<StudentOpeningResponseDto> result = [];

        foreach (var opening in openings)
        {
            bool eligible = IsStudentEligible(student, opening);

            bool hasApplied = await _context.Applications.AnyAsync(a =>
                    a.StudentId == student.Id &&
                    a.OpeningId == opening.Id);
            
            result.Add(new StudentOpeningResponseDto
            {
                OpeningId = opening.Id,
                CompanyName = opening.Company.Name,
                Role = opening.Role,
                Stipend = opening.Stipend,
                CTC = opening.CTC,
                MinCGPA = opening.MinCGPA,
                MinTenthPercentage = opening.MinTenthPercentage,
                MinTwelfthPercentage = opening.MinTwelfthPercentage,
                AllowedBranches = opening.AllowedBranches,
                MaxAge = opening.MaxAge,
                ApplicationDeadline = opening.ApplicationDeadline,
                IsEligible = eligible,
                HasApplied = hasApplied
            });
        }
        return result;
    }

    public async Task<ServiceResponseDto> ApplyToOpeningAsync(int userId, int openingId)
    {
        // Get student
        Student? student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Student not found."
            };
        }
        // Profile must be completed
        if (!student.IsProfileCompleted)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Complete your profile before applying."
            };
        }
        // Get opening
        Opening? opening = await _context.Openings.Include(o => o.Company)
            .FirstOrDefaultAsync(o => o.Id == openingId);

        if (opening == null)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Opening not found."
            };
        }
        // Deadline check
        if (opening.ApplicationDeadline <= DateTime.UtcNow)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Application deadline has passed."
            };
        }
        // Already applied?
        bool alreadyApplied = await _context.Applications
            .AnyAsync(a => a.StudentId == student.Id && a.OpeningId == openingId);

        if (alreadyApplied)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "You have already applied to this opening."
            };
        }
        // Max participants check
        if (opening.MaxParticipants.HasValue)
        {
            int applicationCount = await _context.Applications.CountAsync(a => a.OpeningId == openingId);
            if (applicationCount >= opening.MaxParticipants.Value)
            {
                return new ServiceResponseDto
                {
                    Success = false,
                    Message = "Application limit reached."
                };
            }
        }
        // Dream offer rule
        if (student.IsPlaced)
        {
            if (!opening.CTC.HasValue)
            {
                return new ServiceResponseDto
                {
                    Success = false,
                    Message = "Placed students cannot apply for internship-only openings."
                };
            }
            if (!student.PlacedCTC.HasValue)
            {
                return new ServiceResponseDto
                {
                    Success = false,
                    Message = "Placed CTC information missing."
                };
            }
            if (opening.CTC <= student.PlacedCTC.Value * 1.5m)
            {
                return new ServiceResponseDto
                {
                    Success = false,
                    Message = "Only dream offers above 1.5x your current package are allowed."
                };
            }
        }

        // Eligibility check
        bool eligible = IsStudentEligible(student,opening);

        if (!eligible)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "You are not eligible for this opening."
            };
        }

        // Create application
        Application application = new()
        {
            StudentId = student.Id,
            OpeningId = opening.Id,
            AppliedOn = DateOnly.FromDateTime(DateTime.UtcNow),
            IsSelected = false
        };

        _context.Applications.Add(application);

        await _context.SaveChangesAsync();

        return new ServiceResponseDto
        {
            Success = true,
            Message = "Application submitted successfully."
        };
    }

    public async Task<List<StudentApplicationResponseDto>> GetApplicationsAsync(int userId)
    {
        Student? student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
        {
            return [];
        }

        return await _context.Applications.Where(a => a.StudentId == student.Id)
            .Select(a => new StudentApplicationResponseDto
            {
                ApplicationId = a.Id,
                CompanyName = a.Opening.Company.Name,
                Role = a.Opening.Role,
                AppliedOn = a.AppliedOn,
                IsSelected = a.IsSelected
            }).ToListAsync();
    }

    private bool IsStudentEligible(Student student, Opening opening)
    {
        bool eligible =
            (student.CGPA ?? 0m) >= opening.MinCGPA &&
            (student.TenthPercentage ?? 0m) >= opening.MinTenthPercentage &&
            (student.TwelfthPercentage ?? 0m) >= opening.MinTwelfthPercentage;

        // Branch check
        if (!string.IsNullOrWhiteSpace(opening.AllowedBranches))
        {
            var allowedBranches = opening.AllowedBranches
                .Split(',', StringSplitOptions.RemoveEmptyEntries).Select(b => b.Trim());
            eligible &= student.Branch != null && allowedBranches.Contains(student.Branch);
        }
        // Age check
        if (opening.MaxAge.HasValue)
        {
            if (!student.DateOfBirth.HasValue)
        {
            return false;
        }

        // Unwrap the value safely using .Value
        DateOnly dob = student.DateOfBirth.Value;

        int age = DateTime.Today.Year - dob.Year;
        if (dob.ToDateTime(TimeOnly.MinValue) > DateTime.Today.AddYears(-age))
        {
            age--;
        }
        eligible &= age <= opening.MaxAge.Value;
        }
        return eligible;
    }

    private static readonly HashSet<string> ValidBranches =
    [
        "CSE",
        "ISE",
        "ECE",
        "EEE",
        "EIE",
        "ME",
        "CV",
        "BT"
    ];
}