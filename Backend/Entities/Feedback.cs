using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;

public class Feedback
{
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [Required]
    public int GraduationYear { get; set; }

    [Required]
    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal? CTC { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? Stipend { get; set; }

    [Required]
    [MaxLength(5000)]
    public string ProcessDescription { get; set; } = string.Empty;

    [Required]
    [MaxLength(3000)]
    public string Advice { get; set; } = string.Empty;

    [ForeignKey("StudentId")]
    public Student Student { get; set; } = null!;

    [ForeignKey("CompanyId")]
    public Company Company { get; set; } = null!;
}