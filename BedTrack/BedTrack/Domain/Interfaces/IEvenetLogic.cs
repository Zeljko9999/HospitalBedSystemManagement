using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;

namespace BedTrack.Domain.Interfaces
{
    public interface IEvenetLogic
    {
        Task CreateNewEvent(NewEventDTO? eventt);

        Task<EventDTO> GetEvent(int id);

        Task UpdateEvent(int id, NewEventDTO? updatedEvent);

        Task DeleteEvent(int id);
    }
}
