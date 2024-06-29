using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;

namespace BedTrack.Domain.Interfaces
{
    public interface IUserEventLogic
    {
        Task CreateNewUserEvent(NewUserEventDTO? userEvent);

        Task<IEnumerable<EventDTO>> GetUserEvents (int userId);

        Task<EventDTO?> GetUserEvent(int userId);
        Task<UserEventDTO?> GetUserEventRow(int id);

        Task UpdateUserEvent(int id, NewUserEventDTO? updatedUserEvent);

        Task DeleteUserEvent(int id);
    }
}
