using BedTrack.Domain.Models;

namespace BedTrack.Application.DTO
{
    public class ClinicDepartmentDTO
    {
        public int Id { get; set; }
        public int ClinicId { get; set; }
        public string Clinic { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public int BedQuantity { get; set; }
        public string? Description { get; set; }

        public static ClinicDepartmentDTO FromModel(ClinicDepartment model)
        {
            return new ClinicDepartmentDTO
            {
                Id = model.Id,
                ClinicId = model.ClinicId,
                Clinic = model.Clinic.Name,
                DepartmentId = model.DepartmentId,
                Department = model.Department.Name,
                BedQuantity = model.BedQuantity,
                Description = model.Description,
            };
        }
    }
}
