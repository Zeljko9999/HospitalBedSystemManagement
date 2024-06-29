using BedTrack.Domain.Models;

namespace BedTrack.Application.NewDTO
{
    public class NewClinicDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Clinic ToModel()
        {
            return new Clinic
            {
                Name = Name,
                Description = Description,
                User = null,
                ClinicDepartments = null
            };
        }
    }
}
