using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Admin;

public class UpdatePlacementSettingsDto
{
    [Range(0, 100)]
    public decimal MinCTCDifferencePercentage { get; set; }
}