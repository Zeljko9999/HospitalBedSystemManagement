using BedTrack.Domain.Models;

namespace BedTrack.Application.NewDTO
{
    public class NewUserEventDTO
    {
        public int UserId { get; set; }
        public int EventId { get; set; }

        public UserEvent ToModel()
        {
            return new UserEvent
            {
                UserId = UserId,
                EventId = EventId,
            };
        }
    }
}
