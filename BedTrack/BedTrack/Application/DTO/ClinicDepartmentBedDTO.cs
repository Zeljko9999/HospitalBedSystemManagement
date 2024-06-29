using BedTrack.Domain.Models;

namespace BedTrack.Application.DTO
{
    public class ClinicDepartmentBedDTO
    {
        public int Id { get; set; }
        public bool IsAvailable { get; set; }
        public string? Status { get; set; }
        public string Clinic { get; set; }
        public string Department { get; set; }
        public int ClinicDepartmentId { get; set; }            
        public string Bed { get; set; }
        public int BedId { get; set; }        
        public string? Patient { get; set; }
        public int? PatientId { get; set; }

        public static ClinicDepartmentBedDTO FromModel(ClinicDepartmentBed model)
        {
            return new ClinicDepartmentBedDTO
            {
                Id = model.Id,
                IsAvailable = model.IsAvailable,
                Status = model.Status,
                Clinic = model.ClinicDepartment.Clinic.Name,
                Department = model.ClinicDepartment.Department.Name,
                ClinicDepartmentId = model.ClinicDepartmentId,
                Bed = model.Bed.Name,
                BedId = model.BedId,
                Patient = model.Patient?.Name,
                PatientId = model.PatientId,
            };
        }
    }
}
