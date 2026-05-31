using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;

public class Application
{
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int OpeningId { get; set; }

    [ForeignKey("StudentId")]
    public Student Student { get; set; } = null!;

    [ForeignKey("OpeningId")]
    public Opening Opening { get; set; } = null!;
}