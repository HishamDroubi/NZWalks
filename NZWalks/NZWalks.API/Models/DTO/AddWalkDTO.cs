namespace NZWalks.API.Models.Domain
{
    public class AddWalkDTO
    {
        public string Name { get; set; }

        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }
        public string Description { get; set; }

        public Guid RegionId { get; set; }

        public Guid DifficultyId { get; set; }

    }
}
