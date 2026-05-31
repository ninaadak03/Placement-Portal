using Backend.Data;
using Backend.Entities;
using Backend.DTOs.Auth;
using Backend.DTOs.Company;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class CompanyService : ICompanyService
{
    private readonly ApplicationDbContext _context;

    public CompanyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponseDto> CreateCompanyAsync(CreateCompanyDto dto)
    {
        dto.Name = (dto.Name ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Company name is required."
            };
        }

        bool companyExists = await _context.Companies.AnyAsync(c => c.Name == dto.Name);

        if (companyExists)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Company already exists."
            };
        }

        Company company = new()
        {
            Name = dto.Name.Trim()
        };

        _context.Companies.Add(company);

        await _context.SaveChangesAsync();

        return new ServiceResponseDto
        {
            Success = true,
            Message = "Company created successfully."
        };
    }

    public async Task<List<CompanyResponseDto>> GetAllCompaniesAsync()
    {
        return await _context.Companies.Select(c => new CompanyResponseDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();
    }

    public async Task<CompanyResponseDto?> GetCompanyByIdAsync(int companyId)
    {
        return await _context.Companies.Where(c => c.Id == companyId)
            .Select(c => new CompanyResponseDto
            {
                Id = c.Id,
                Name = c.Name
            }).FirstOrDefaultAsync();
    }

    public async Task<ServiceResponseDto> DeleteCompanyAsync(int companyId)
    {
        Company? company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId);

        if (company == null)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Company not found."
            };
        }

        bool hasOpenings = await _context.Openings.AnyAsync(o => o.CompanyId == companyId);
        if (hasOpenings)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Cannot delete company with existing openings."
            };
        }

        _context.Companies.Remove(company);

        await _context.SaveChangesAsync();

        return new ServiceResponseDto
        {
            Success = true,
            Message = "Company deleted successfully."
        };
    }

    public async Task<ServiceResponseDto> UpdateCompanyAsync(int companyId, UpdateCompanyDto dto)
    {
        dto.Name = (dto.Name ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Company name is required."
            };
        }

        Company? company = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == companyId);

        if (company == null)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Company not found."
            };
        }

        bool companyExists = await _context.Companies
            .AnyAsync(c =>
                c.Id != companyId &&
                c.Name == dto.Name);

        if (companyExists)
        {
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Company already exists."
            };
        }

        company.Name = dto.Name;

        await _context.SaveChangesAsync();

        return new ServiceResponseDto
        {
            Success = true,
            Message = "Company updated successfully."
        };
    }
}