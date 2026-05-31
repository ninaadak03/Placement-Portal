using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Company;

public class CreateCompanyDto
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
}