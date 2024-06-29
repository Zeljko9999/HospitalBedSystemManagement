using BedTrack.Domain.Models;

namespace BedTrack.DAL.Interfaces
{
    public interface IEventRepository
    {
        Task AddEvent(Event? eventt);

        public Task<Event> GetEvent(int id);

        Task UpdateEvent(Event? updatedEvent);

        Task DeleteEvent(int id);
    }
}
