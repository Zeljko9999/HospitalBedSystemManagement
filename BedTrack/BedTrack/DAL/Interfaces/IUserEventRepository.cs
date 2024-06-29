using BedTrack.Domain.Models;

namespace BedTrack.DAL.Interfaces
{
    public interface IUserEventRepository
    {
        Task AddUserEvent(UserEvent? userEvent);

        Task<List<Event>> GetAllEventsOfUser(int userId);

        public Task<Event> GetEventForUser(int userId);
        public Task<UserEvent> GetUserEvent(int id);

        Task UpdateUserEvent(UserEvent? updatedUserEvent);

        Task DeleteUserEvent(int id);
    }
}
