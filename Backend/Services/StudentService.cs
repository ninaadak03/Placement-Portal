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

            Name = student.Name,
            PhoneNumber = student.PhoneNumber,
            Branch = student.Branch,
            Gender = student.Gender,
            DateOfBirth = student.DateOfBirth,

            TenthPercentage = student.TenthPercentage,
            TwelfthPercentage = student.TwelfthPercentage,

            SgpaSem1 = student.SgpaSem1,
            SgpaSem2 = student.SgpaSem2,
            SgpaSem3 = student.SgpaSem3,
            SgpaSem4 = student.SgpaSem4,
            SgpaSem5 = student.SgpaSem5,
            SgpaSem6 = student.SgpaSem6,
            SgpaSem7 = student.SgpaSem7,
            SgpaSem8 = student.SgpaSem8,

            CGPA = student.CGPA,

            ResumeUrl = student.ResumeUrl,

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