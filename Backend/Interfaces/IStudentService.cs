using Backend.DTOs.Auth;
using Backend.DTOs.Student;

namespace Backend.Interfaces;

public interface IStudentService
{
    Task<StudentProfileResponseDto> GetProfileAsync(int userId);

    Task<ServiceResponseDto> CompleteProfileAsync(int userId,CompleteProfileDto dto);

    Task<List<StudentOpeningResponseDto>> GetAvailableOpeningsAsync(int userId);

    Task<ServiceResponseDto> ApplyToOpeningAsync(int userId, int openingId);

    Task<List<StudentApplicationResponseDto>> GetApplicationsAsync(int userId);
}