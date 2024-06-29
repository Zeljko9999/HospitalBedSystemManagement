using BedTrack.Domain.Models;

namespace BedTrack.Application.DTO
{
    public class ClinicDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static ClinicDTO FromModel(Clinic model)
        {
            return new ClinicDTO
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
            };
        }
    }
}
