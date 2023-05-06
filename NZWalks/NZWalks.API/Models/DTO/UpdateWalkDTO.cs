using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.Domain
{
    public class UpdateWalkDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }
        public string? Description { get; set; }

        [Required]
        public Guid RegionId { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

    }
}
