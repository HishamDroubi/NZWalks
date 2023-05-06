using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Globalization;
using System.Security.Cryptography.Xml;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {

        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn, string? filterQuery,
            string? sortBy, bool isAscending ,int pageNumber,int pageSize)
        {
            
            var walks= dbContext.Walks
                .Include("Region")
                .Include("Difficulty").AsQueryable();

          
            //Filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery)){

                //Filter For Name
                if (filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x=>x.Name.Contains(filterQuery));
                }

                //Filter For Description
                if (filterOn.Equals("description", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Description.Contains(filterQuery));
                }
            }


            //Sotring
            if (!string.IsNullOrWhiteSpace(sortBy))
            {

                //Sorting On Name
                if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    walks =  isAscending ? walks.OrderBy(x => x.Name): walks.OrderByDescending(x => x.Name);
                }

                //Sorting On Description
                if (sortBy.Equals("length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }


            //Pagination
            if (pageSize!=0)
            {
                int walksToSkip = (pageNumber - 1) * pageSize;
                walks= walks.Skip(walksToSkip).Take(pageSize);
            }


            return await walks.ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        { 
            return await dbContext.Walks
                .Include("Region")
                .Include("Difficulty").FirstAsync(x => x.Id == id);
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            Walk existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (walk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.Description = walk.Description;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.DifficultyId = walk.DifficultyId;

            await dbContext.SaveChangesAsync();
            return walk;
            
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {

            Walk walk=dbContext.Walks.FirstOrDefault(x => x.Id == id);
            if(walk == null)
            {
                return null;
            }

            dbContext.Walks.Remove(walk);
            await dbContext.SaveChangesAsync();
            return walk; 
        }




      
    }
}
