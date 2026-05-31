using Backend.DTOs.Auth;
using Backend.DTOs.Opening;

namespace Backend.Interfaces;

public interface IOpeningService
{
    Task<ServiceResponseDto> CreateOpeningAsync(CreateOpeningDto dto);

    Task<List<OpeningResponseDto>> GetAllOpeningsAsync();

    Task<OpeningResponseDto?> GetOpeningByIdAsync(int openingId);

    Task<ServiceResponseDto> UpdateOpeningAsync(
        int openingId,
        UpdateOpeningDto dto);

    Task<ServiceResponseDto> DeleteOpeningAsync(int openingId);
}