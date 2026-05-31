using Backend.DTOs.Auth;

namespace Backend.Interfaces;

public interface IAuthService
{
    Task<ServiceResponseDto> RegisterAsync(StudentRegisterDto dto);

    Task<ServiceResponseDto> VerifyOtpAsync(VerifyOtpDto dto);

    Task<AuthResponseDto> StudentLoginAsync(StudentLoginDto dto);

    Task<AuthResponseDto> AdminLoginAsync(AdminLoginDto dto);
}