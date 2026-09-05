using Backend.Data;
using Backend.DTOs.Auth;
using Backend.Interfaces;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IJwtService _jwtService;

    public AuthService(ApplicationDbContext context, IEmailService emailService, IJwtService jwtService)
    {
        _context = context;
        _emailService = emailService;
        _jwtService = jwtService;
    }

    public async Task<ServiceResponseDto> RegisterAsync(StudentRegisterDto dto)
    {
        bool emailExists = await _context.Users.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower());
        if (emailExists)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Email is already registered."
            };
        }

        bool rollNoExists = await _context.Users.AnyAsync(u => u.RollNo == dto.RollNo);
        if (rollNoExists)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Roll number is already registered."
            };
        }

        string otp = Random.Shared.Next(100000, 1000000).ToString();

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var existingOtp = await _context.OtpVerifications.FirstOrDefaultAsync(o => o.Email == dto.Email);
        if (existingOtp != null)
        {
            existingOtp.RollNo = dto.RollNo;
            existingOtp.PasswordHash = passwordHash;
            existingOtp.OtpCode = otp;
            existingOtp.ExpiryTime = DateTime.UtcNow.AddMinutes(6);
            existingOtp.IsUsed = false;
        }
        else
        {
            OtpVerification otpVerification = new()
            {
                Email = dto.Email,
                RollNo = dto.RollNo,
                PasswordHash = passwordHash,
                OtpCode = otp,
                ExpiryTime = DateTime.UtcNow.AddMinutes(6),
                IsUsed = false
            };
            _context.OtpVerifications.Add(otpVerification);
        }
        await _context.SaveChangesAsync();
        
        try
        {
            await _emailService.SendOtpAsync(dto.Email, otp);
            return new ServiceResponseDto
            {
                Success = true,
                Message = "OTP sent successfully."
            };
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ServiceResponseDto
            {
                Success = false,
                Message = "OTP sending failed."
            };
        }
    }

    public async Task<ServiceResponseDto> VerifyOtpAsync(VerifyOtpDto dto)
    {
        var otpEntry = await _context.OtpVerifications.FirstOrDefaultAsync(o => o.Email == dto.Email);

        if (otpEntry == null)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "No OTP request found for this email."
            };
        }

        if (otpEntry.IsUsed)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "OTP has already been used."
            };
        }

        if (otpEntry.ExpiryTime < DateTime.UtcNow)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "OTP has expired."
            };
        }

        if (otpEntry.OtpCode != dto.OtpCode)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Invalid OTP."
            };
        }
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            User user = new()
            {
                Email = otpEntry.Email,
                RollNo = otpEntry.RollNo,
                PasswordHash = otpEntry.PasswordHash,
                Role = "Student",
                IsVerified = true
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            Student student = new()
            {
                UserId = user.Id,
                RollNo = otpEntry.RollNo,
                IsProfileCompleted = false,
                IsPlaced = false
            };
            _context.Students.Add(student);
            otpEntry.IsUsed = true;
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new ServiceResponseDto
            {
                Success = true,
                Message = "Account verified successfully."
            };
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync();

            return new ServiceResponseDto
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<AuthResponseDto> StudentLoginAsync(StudentLoginDto dto)
    {
        User? user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.RollNo == dto.RollNo &&
                u.Role == "Student");

        if (user == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid credentials."
            };
        }

        bool passwordValid =
            BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash);

        if (!passwordValid)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid credentials."
            };
        }

        Student? student = await _context.Students
            .FirstOrDefaultAsync(s => s.UserId == user.Id);

        if (student == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Student not found."
            };
        }

        return new AuthResponseDto
        {
            Success = true,
            Token = _jwtService.GenerateToken(user),
            Role = user.Role,
            IsProfileCompleted = student.IsProfileCompleted
        };
    }

    public async Task<AuthResponseDto> AdminLoginAsync(AdminLoginDto dto)
    {
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == dto.Email &&
                    u.Role == "Admin");

            if (user == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid credentials."
                };
            }

            bool passwordValid =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    user.PasswordHash);

            if (!passwordValid)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid credentials."
                };
            }

            return new AuthResponseDto
            {
                Success = true,
                Token = _jwtService.GenerateToken(user),
                Role = user.Role,
                IsProfileCompleted = true
            };
        }
    }
}