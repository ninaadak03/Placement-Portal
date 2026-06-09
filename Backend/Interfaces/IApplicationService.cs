using Backend.DTOs.Auth;
using Backend.DTOs.Application;

namespace Backend.Interfaces;

public interface IApplicationService
{
    Task<List<ApplicationResponseDto>> GetApplicationsForOpeningAsync(int openingId);

    Task<ServiceResponseDto> SelectStudentAsync(int applicationId);
}