using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {

        Task<List<Walk>> GetAllAsync(string? filterOn, string? filterQuery,
            string? sortBy, bool isAscending, int pageNumber, int pageSize);

        Task<Walk?> GetByIdAsync(Guid id);

        Task<Walk> CreateAsync(Walk walk);

        Task<Walk?> UpdateAsync(Guid id,Walk walk);
        Task<Walk?> DeleteAsync(Guid id);



    }
}
