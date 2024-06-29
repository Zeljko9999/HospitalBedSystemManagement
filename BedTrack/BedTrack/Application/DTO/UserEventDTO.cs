using BedTrack.Domain.Models;

namespace BedTrack.Application.DTO
{
    public class UserEventDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }

        public static UserEventDTO FromModel(UserEvent model)
        {
            return new UserEventDTO
            {
                Id = model.Id,
                UserId = model.UserId,
                EventId = model.EventId,
            };
        }
    }
}
