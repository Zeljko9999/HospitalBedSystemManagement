namespace BedTrack.Domain.Models
{
    public class Bed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ClinicDepartmentBed> ClinicDepartmentBeds { get; set; }
    }
}