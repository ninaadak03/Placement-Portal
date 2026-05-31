using Backend.DTOs.Auth;
using Backend.DTOs.Student;

namespace Backend.Interfaces;

public interface IStudentService
{
    Task<StudentProfileResponseDto> GetProfileAsync(int userId);

    Task<ServiceResponseDto> CompleteProfileAsync(int userId,CompleteProfileDto dto);
}