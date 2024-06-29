namespace BedTrack.Domain.Models
{
    public class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<User>? User { get; set; }
        public ICollection<ClinicDepartment>? ClinicDepartments { get; set; }
    }
}