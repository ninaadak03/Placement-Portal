using System.ComponentModel.DataAnnotations;

namespace Backend.Entities;

public class Company
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}