namespace BedTrack.Domain.Models
{
    public class UserPatient
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
