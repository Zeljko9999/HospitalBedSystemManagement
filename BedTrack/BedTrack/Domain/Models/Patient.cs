namespace BedTrack.Domain.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Nationality { get; set; }
        public int? Age { get; set; }
        public string? Insurance { get; set; }
        public string? HealthRecord { get; set; }
        public string? HealthHistory { get; set; }

        public int ClinicDepartmentBedId { get; set; }
        public ClinicDepartmentBed ClinicDepartmentBed { get; set; }
        public ICollection<UserPatient>? Users { get; set; }
    }
}
