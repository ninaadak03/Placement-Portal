using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Backend.Entities;

public class PlacementSettings
{
    public int Id { get; set; }

    public bool BlockPlacedStudents { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    [Range(0, 100)]
    public decimal MinCTCDifferencePercentage { get; set; }
}