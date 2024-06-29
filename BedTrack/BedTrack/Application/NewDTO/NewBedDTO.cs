using BedTrack.Domain.Models;

namespace BedTrack.Application.NewDTO
{
    public class NewBedDTO
    {
        public string Name { get; set; }

        public Bed ToModel()
        {
            return new Bed
            {
                Name = Name,
            };
        }
    }
}
