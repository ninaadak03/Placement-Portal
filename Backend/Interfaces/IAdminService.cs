using Backend.DTOs.Student;
using Backend.DTOs.Admin;

namespace Backend.Interfaces;

public interface IAdminService
{
    Task<List<AdminStudentResponseDto>> GetStudentsAsync(string? branch, bool? isPlaced, int? companyId);

    Task<AdminStudentDetailResponseDto?> GetStudentByIdAsync(int studentId);

    Task<List<AdminStudentDetailResponseDto>> GetStudentDetailsAsync(string? branch, bool? isPlaced, int? companyId);

    Task<AdminDashboardResponseDto> GetDashboardAsync();
}