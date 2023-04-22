using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class InMemoryRegionRepository : IRegionRepository
    {
        public void CreateAsync(Region region)
        {
            throw new NotImplementedException();
        }

        public Task<Region?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return new List<Region>();
        }

        public Task<Region> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Region?> UpdateAsync(Guid id, Region region)
        {
            throw new NotImplementedException();
        }

        Task<Region> IRegionRepository.CreateAsync(Region region)
        {
            throw new NotImplementedException();
        }
    }
}
