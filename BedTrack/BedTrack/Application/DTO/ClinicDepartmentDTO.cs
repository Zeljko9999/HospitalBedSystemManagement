using BedTrack.Domain.Models;

namespace BedTrack.Application.DTO
{
    public class ClinicDepartmentDTO
    {
        public int Id { get; set; }
        public string Clinic { get; set; }

        public string Department { get; set; }
        public int BedQuantity { get; set; }
        public string Description { get; set; }

        public static ClinicDepartmentDTO FromModel(ClinicDepartment model)
        {
            return new ClinicDepartmentDTO
            {
                Id = model.Id,
                Clinic = model.Clinic.Name,
                Department = model.Department.Name,
                BedQuantity = model.BedQuantity,
                Description = model.Description,
            };
        }
    }
}
