using BedTrack.Domain.Models;

namespace BedTrack.Application.NewDTO
{
    public class NewEventDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Alarm { get; set; }

        public Event ToModel()
        {
            return new Event
            {
                Name = Name,
                Description = Description,
                Alarm = Alarm,
                AlarmEvents = null,
            };
        }
    }
}
