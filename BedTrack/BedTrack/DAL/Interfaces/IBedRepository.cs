using BedTrack.Domain.Models;

namespace BedTrack.DAL.Interfaces
{
    public interface IBedRepository
    {
        Task AddBed(Bed? bed);

        Task<List<Bed>> GetAllBeds();

        public Task<Bed> GetBed(int id);

        Task UpdateBed(Bed? updatedBed);

        Task DeleteBed(int id);
    }
}
