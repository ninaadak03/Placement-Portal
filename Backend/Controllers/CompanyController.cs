using Backend.DTOs.Company;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto dto)
    {
        var result = await _companyService.CreateCompanyAsync(dto);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCompanies()
    {
        var companies = await _companyService.GetAllCompaniesAsync();
        return Ok(companies);
    }

    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetCompanyById(int companyId)
    {
        var company = await _companyService.GetCompanyByIdAsync(companyId);
        if (company == null)
        {
            return NotFound(new
            {
                Message = "Company not found."
            });
        }
        return Ok(company);
    }

    [HttpPut("{companyId}")]
    public async Task<IActionResult> UpdateCompany(int companyId,[FromBody] UpdateCompanyDto dto)
    {
        var result = await _companyService.UpdateCompanyAsync(companyId, dto);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpDelete("{companyId}")]
    public async Task<IActionResult> DeleteCompany(int companyId)
    {
        var result = await _companyService.DeleteCompanyAsync(companyId);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}