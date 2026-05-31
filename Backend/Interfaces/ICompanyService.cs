using Backend.DTOs.Auth;
using Backend.DTOs.Company;

namespace Backend.Interfaces;

public interface ICompanyService
{
    Task<ServiceResponseDto> CreateCompanyAsync(CreateCompanyDto dto);

    Task<ServiceResponseDto> UpdateCompanyAsync(int companyId,UpdateCompanyDto dto);
    
    Task<List<CompanyResponseDto>> GetAllCompaniesAsync();

    Task<CompanyResponseDto?> GetCompanyByIdAsync(int companyId);

    Task<ServiceResponseDto> DeleteCompanyAsync(int companyId);
}