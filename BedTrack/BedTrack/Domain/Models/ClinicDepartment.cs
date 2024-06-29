namespace BedTrack.Domain.Models
{
    public class ClinicDepartment
    {
        public int Id { get; set; }
        public int BedQuantity { get; set; }
        public string? Description { get; set; }

        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; } 

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<ClinicDepartmentBed> ClinicDepartmentBeds { get; set; }
    }
}
