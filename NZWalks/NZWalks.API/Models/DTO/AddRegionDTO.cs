
using System.ComponentModel.DataAnnotations;

public class AddRegionDTO
    {
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(3)]
    [MinLength(3)]
    public string Code { get; set; }
    public string? RegionImageUrl { get; set; }

    }

