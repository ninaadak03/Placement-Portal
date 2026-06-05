using System.Security.Claims;
using Backend.DTOs.Student;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Student")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var profile = await _studentService.GetProfileAsync(GetUserId());
        return Ok(profile);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> CompleteProfile([FromBody] CompleteProfileDto dto)
    {
        var result = await _studentService.CompleteProfileAsync(GetUserId(), dto);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet("openings")]
    public async Task<IActionResult> GetAvailableOpenings()
    {
        var openings = await _studentService
            .GetAvailableOpeningsAsync(GetUserId());

        return Ok(openings);
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}