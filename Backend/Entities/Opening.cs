using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;

public class Opening
{
    public int Id { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal? CTC { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? Stipend { get; set; }

    public int? MaxParticipants { get; set; }

    [Column(TypeName = "decimal(4,2)")]
    [Range(0, 10)]
    public decimal MinCGPA { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    [Range(0, 100)]
    public decimal MinTenthPercentage { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    [Range(0, 100)]
    public decimal MinTwelfthPercentage { get; set; }

    [MaxLength(200)]
    public string? AllowedBranches { get; set; }

    public int? MaxAge { get; set; }

    [Required]
    public DateTime ApplicationDeadline { get; set; }

    [ForeignKey("CompanyId")]
    public Company Company { get; set; } = null!;
}