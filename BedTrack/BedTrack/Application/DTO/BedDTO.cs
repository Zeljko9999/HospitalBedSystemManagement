using BedTrack.Domain.Models;

namespace BedTrack.Application.DTO
{
    public class BedDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static BedDTO FromModel(Bed model)
        {
            return new BedDTO
            {
                Id = model.Id,
                Name = model.Name,
            };
        }
    }
}
