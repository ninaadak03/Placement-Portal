using Backend.DTOs.Auth;
using Backend.DTOs.Feedback;

namespace Backend.Interfaces;

public interface IFeedbackService
{
    Task<ServiceResponseDto> CreateFeedbackAsync(int userId, CreateFeedbackDto dto);

    Task<List<FeedbackResponseDto>> GetFeedbackAsync(string? studentName, string? companyName);

    Task<ServiceResponseDto> DeleteFeedbackAsync(int userId, string role, int feedbackId);
}