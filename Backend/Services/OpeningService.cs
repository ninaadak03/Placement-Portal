using Backend.Data;
using Backend.DTOs.Auth;
using Backend.DTOs.Opening;
using Backend.Interfaces;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class OpeningService : IOpeningService
{
    private readonly ApplicationDbContext _context;

    public OpeningService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponseDto> CreateOpeningAsync(CreateOpeningDto dto)
    {
        bool companyExists = await _context.Companies.AnyAsync(c => c.Id == dto.CompanyId);
        if (!companyExists)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Company not found."
            };
        }

        dto.Role = dto.Role.Trim();
        if (string.IsNullOrWhiteSpace(dto.Role))
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Role is required."
            };
        }
        if (dto.ApplicationDeadline <= DateTime.UtcNow)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Application deadline must be in the future."
            };
        }

        if (!string.IsNullOrWhiteSpace(dto.AllowedBranches))
        {
            string[] branches = dto.AllowedBranches.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (string branch in branches)
            {
                if (!ValidBranches.Contains(branch.Trim()))
                {
                    return new ServiceResponseDto
                    {
                        Success = false,
                        Message = $"Invalid branch: {branch}"
                    };
                }
            }
            dto.AllowedBranches = string.Join("," , branches.Select(b => b.Trim()));
        }
        Opening opening = new()
        {
            CompanyId = dto.CompanyId,
            Role = dto.Role,
            Stipend = dto.Stipend,
            CTC = dto.CTC,
            MaxParticipants = dto.MaxParticipants,
            MinCGPA = dto.MinCGPA,
            MinTenthPercentage = dto.MinTenthPercentage,
            MinTwelfthPercentage = dto.MinTwelfthPercentage,
            AllowedBranches = dto.AllowedBranches,
            MaxAge = dto.MaxAge,
            ApplicationDeadline = dto.ApplicationDeadline
        };

        _context.Openings.Add(opening);
        await _context.SaveChangesAsync();
        return new ServiceResponseDto
        {
            Success = true,
            Message = "Opening created successfully."
        };
    }

    public async Task<List<OpeningResponseDto>> GetAllOpeningsAsync()
    {
        return await _context.Openings.Include(o => o.Company)
            .Select(o => new OpeningResponseDto
            {
                Id = o.Id,
                CompanyId = o.CompanyId,
                CompanyName = o.Company.Name,
                Role = o.Role,
                Stipend = o.Stipend,
                CTC = o.CTC,
                MaxParticipants = o.MaxParticipants,
                MinCGPA = o.MinCGPA,
                MinTenthPercentage = o.MinTenthPercentage,
                MinTwelfthPercentage = o.MinTwelfthPercentage,
                AllowedBranches = o.AllowedBranches,
                MaxAge = o.MaxAge,
                ApplicationDeadline = o.ApplicationDeadline
            }).ToListAsync();
    }

    public async Task<OpeningResponseDto?> GetOpeningByIdAsync(int openingId)
    {
        return await _context.Openings.Include(o => o.Company)
            .Where(o => o.Id == openingId)
            .Select(o => new OpeningResponseDto
            {
                Id = o.Id,
                CompanyId = o.CompanyId,
                CompanyName = o.Company.Name,
                Role = o.Role,
                Stipend = o.Stipend,
                CTC = o.CTC,
                MaxParticipants = o.MaxParticipants,
                MinCGPA = o.MinCGPA,
                MinTenthPercentage = o.MinTenthPercentage,
                MinTwelfthPercentage = o.MinTwelfthPercentage,
                AllowedBranches = o.AllowedBranches,
                MaxAge = o.MaxAge,
                ApplicationDeadline = o.ApplicationDeadline
            }).FirstOrDefaultAsync();
    }

    public async Task<ServiceResponseDto> UpdateOpeningAsync(int openingId, UpdateOpeningDto dto)
    {
        Opening? opening = await _context.Openings.FirstOrDefaultAsync(o => o.Id == openingId);

        if (opening == null)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Opening not found."
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

        dto.Role = dto.Role.Trim();

        if (string.IsNullOrWhiteSpace(dto.Role))
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Role is required."
            };
        }

        if (dto.ApplicationDeadline <= DateTime.UtcNow)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Application deadline must be in the future."
            };
        }

        if (!string.IsNullOrWhiteSpace(dto.AllowedBranches))
        {
            string[] branches = dto.AllowedBranches
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (string branch in branches)
            {
                if (!ValidBranches.Contains(branch.Trim()))
                {
                    return new ServiceResponseDto
                    {
                        Success = false,
                        Message = $"Invalid branch: {branch}"
                    };
                }
            }

            dto.AllowedBranches = string.Join(",",
                branches.Select(b => b.Trim()));
        }

        opening.CompanyId = dto.CompanyId;
        opening.Role = dto.Role;
        opening.Stipend = dto.Stipend;
        opening.CTC = dto.CTC;
        opening.MaxParticipants = dto.MaxParticipants;
        opening.MinCGPA = dto.MinCGPA;
        opening.MinTenthPercentage = dto.MinTenthPercentage;
        opening.MinTwelfthPercentage = dto.MinTwelfthPercentage;
        opening.AllowedBranches = dto.AllowedBranches;
        opening.MaxAge = dto.MaxAge;
        opening.ApplicationDeadline = dto.ApplicationDeadline;

        await _context.SaveChangesAsync();

        return new ServiceResponseDto
        {
            Success = true,
            Message = "Opening updated successfully."
        };
    }

    public Task<ServiceResponseDto> DeleteOpeningAsync(int openingId)
    {
        throw new NotImplementedException();
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