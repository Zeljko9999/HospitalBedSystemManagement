using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;

namespace BedTrack.Domain.Interfaces
{
    public interface IBedLogic
    {
        Task CreateNewBed(NewBedDTO? bed);

        Task<IEnumerable<BedDTO>> GetBeds();

        Task<BedDTO> GetBed(int id);

        Task UpdateBed(int id, NewBedDTO? updatedBed);

        Task DeleteBed(int id);
    }
}
