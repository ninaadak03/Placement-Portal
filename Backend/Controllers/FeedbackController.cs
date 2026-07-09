using System.Security.Claims;
using Backend.DTOs.Feedback;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFeedback([FromQuery] string? studentName, [FromQuery] string? companyName)
    {
        var feedbacks = await _feedbackService.GetFeedbackAsync(studentName, companyName);
        return Ok(feedbacks);
    }

    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> CreateFeedback(CreateFeedbackDto dto)
    {
        var result = await _feedbackService.CreateFeedbackAsync(GetUserId(), dto);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpDelete("{feedbackId}")]
    public async Task<IActionResult> DeleteFeedback(int feedbackId)
    {
        string role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        var result = await _feedbackService.DeleteFeedbackAsync(GetUserId(), role, feedbackId);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}