using BedTrack.Domain.Models;

namespace BedTrack.Application.NewDTO
{
    public class NewClinicDepartmentBedDTO
    {
        public bool IsAvailable { get; set; }
        public string? Status { get; set; }
        public int ClinicDepartmentId { get; set; }
        public int BedId { get; set; }
        public int? PatientId { get; set; }

        public ClinicDepartmentBed ToModel()
        {
            return new ClinicDepartmentBed
            {
                IsAvailable = IsAvailable,
                Status = Status,
                ClinicDepartmentId = ClinicDepartmentId,
                BedId = BedId,
                PatientId = PatientId
            };
        }
    }
}
