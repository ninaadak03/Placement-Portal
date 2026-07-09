using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ApplicationController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public ApplicationController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpGet("opening/{openingId}")]
    public async Task<IActionResult> GetApplicationsForOpening(int openingId)
    {
        var applications = await _applicationService.GetApplicationsForOpeningAsync(openingId);
        return Ok(applications);
    }

    [HttpPost("{applicationId}/select")]
    public async Task<IActionResult> SelectStudent(int applicationId)
    {
        var result = await _applicationService.SelectStudentAsync(applicationId);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}