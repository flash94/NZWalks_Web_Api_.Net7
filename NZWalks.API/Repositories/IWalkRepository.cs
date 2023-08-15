using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>>GetAllAsync(string? filterOn = null, string? filterquery = null);
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> UpdateByIdAsync(Guid id, Walk walk);
        Task<Walk?> DeleteAsync(Guid id);
    }
}
