namespace BedTrack.Domain.Models
{
    public class ClinicDepartmentBed
    {
        public int Id { get; set; }
        public bool IsAvailable { get; set; }
        public string? Status { get; set; }
        public int ClinicDepartmentId { get; set; }
        public ClinicDepartment ClinicDepartment { get; set; }

        public int BedId { get; set; }
        public Bed Bed { get; set; }
        public int? PatientId { get; set; }
        public Patient? Patient { get; set; }
    }
}
