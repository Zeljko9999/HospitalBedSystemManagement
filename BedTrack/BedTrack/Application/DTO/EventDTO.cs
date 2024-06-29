using BedTrack.Domain.Models;

namespace BedTrack.Application.DTO
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Alarm { get; set; }

        public static EventDTO FromModel(Event model)
        {
            return new EventDTO
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Alarm = model.Alarm,
            }; 
        }
    }
}
