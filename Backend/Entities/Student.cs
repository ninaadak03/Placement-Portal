using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;

public class Student
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MinLength(11)]
    [MaxLength(12)]
    public string RollNo { get; set; } = string.Empty;

    [Required]
    [Phone]
    [MaxLength(10)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Branch { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Gender { get; set; } = string.Empty;

    [Required]
    public DateOnly DateOfBirth { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(5,2)")]
    [Range(0, 100)]
    public decimal TenthPercentage { get; set; }

    [Required]
    [Column(TypeName = "decimal(5,2)")]
    [Range(0, 100)]
    public decimal TwelfthPercentage { get; set; }

    [Column(TypeName = "decimal(4,2)")]
    [Range(0, 10)]
    public decimal? SgpaSem1 { get; set; }
    
    [Column(TypeName = "decimal(4,2)")]
    [Range(0, 10)]
    public decimal? SgpaSem2 { get; set; }

    [Column(TypeName = "decimal(4,2)")]
    [Range(0, 10)]
    public decimal? SgpaSem3 { get; set; }

    [Column(TypeName = "decimal(4,2)")]
    [Range(0, 10)]
    public decimal? SgpaSem4 { get; set; }

    [Column(TypeName = "decimal(4,2)")]
    [Range(0, 10)]
    public decimal? SgpaSem5 { get; set; }

    [Column(TypeName = "decimal(4,2)")]
    [Range(0, 10)]
    public decimal? SgpaSem6 { get; set; }

    [Column(TypeName = "decimal(4,2)")]
    [Range(0, 10)]
    public decimal? SgpaSem7 { get; set; }

    [Column(TypeName = "decimal(4,2)")]
    [Range(0, 10)]
    public decimal? SgpaSem8 { get; set; }

    [Required]
    [Column(TypeName = "decimal(4,2)")]
    [Range(0, 10)]
    public decimal CGPA { get; set; }

    [Required]
    [MaxLength(500)]
    public string ResumeUrl { get; set; } = string.Empty;

    public bool IsPlaced { get; set; }

    public int? PlacedCompanyId { get; set; }

    public bool IsProfileCompleted { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? PlacedCTC { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [ForeignKey("PlacedCompanyId")]
    public Company? PlacedCompany { get; set; }
}