using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    // Student list with filters
    [HttpGet("students")]
    public async Task<IActionResult> GetStudents(
        [FromQuery] string? branch,
        [FromQuery] bool? isPlaced,
        [FromQuery] int? companyId)
    {
        var students = await _adminService.GetStudentsAsync(branch,isPlaced,companyId);
        return Ok(students);
    }

    // Single student details
    [HttpGet("students/{studentId}")]
    public async Task<IActionResult> GetStudentById(int studentId)
    {
        var student = await _adminService.GetStudentByIdAsync(studentId);

        if (student == null)
        {
            return NotFound(new
            {
                Message = "Student not found."
            });
        }
        return Ok(student);
    }

    // Detailed list for Excel export
    [HttpGet("students/export")]
    public async Task<IActionResult> GetStudentDetails(
            [FromQuery] string? branch,
            [FromQuery] bool? isPlaced,
            [FromQuery] int? companyId)
    {
        var students = await _adminService.GetStudentDetailsAsync(branch,isPlaced,companyId);
        return Ok(students);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var dashboard = await _adminService.GetDashboardAsync();
        return Ok(dashboard);
    }
}