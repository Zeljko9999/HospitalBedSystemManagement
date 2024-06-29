using BedTrack.Domain.Models;

namespace BedTrack.Application.NewDTO
{
    public class NewDepartmentDTO
    {
        public string Name { get; set; }

        public Department ToModel()
        {
            return new Department
            {
                Name = Name,
                Users = null,
                ClinicDepartments = null
            };
        }
    }
}
