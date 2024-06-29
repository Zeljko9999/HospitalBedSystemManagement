namespace BedTrack.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string? Status { get; set; }

        public int ClinicId { get; set; }
        public Clinic Clicnic { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<UserEvent>? AlarmEvents { get; set; }
        public ICollection<UserPatient>? Patients { get; set; }
    }
}
