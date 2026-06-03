using Backend.DTOs.Opening;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class OpeningController : ControllerBase
{
    private readonly IOpeningService _openingService;

    public OpeningController(IOpeningService openingService)
    {
        _openingService = openingService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOpening([FromBody] CreateOpeningDto dto)
    {
        var result = await _openingService.CreateOpeningAsync(dto);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOpenings()
    {
        var openings = await _openingService.GetAllOpeningsAsync();
        return Ok(openings);
    }

    [HttpGet("{openingId}")]
    public async Task<IActionResult> GetOpeningById(int openingId)
    {
        var opening = await _openingService.GetOpeningByIdAsync(openingId);
        if (opening == null)
        {
            return NotFound(new
            {
                Message = "Opening not found."
            });
        }
        return Ok(opening);
    }

    [HttpPut("{openingId}")]
    public async Task<IActionResult> UpdateOpening(int openingId, [FromBody] UpdateOpeningDto dto)
    {
        var result = await _openingService.UpdateOpeningAsync(openingId, dto);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpDelete("{openingId}")]
    public async Task<IActionResult> DeleteOpening(
        int openingId)
    {
        var result = await _openingService
            .DeleteOpeningAsync(openingId);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}