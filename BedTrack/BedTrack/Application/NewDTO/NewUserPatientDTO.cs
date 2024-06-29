using BedTrack.Domain.Models;

namespace BedTrack.Application.NewDTO
{
    public class NewUserPatientDTO
    {
        public int UserId { get; set; }
        public int PatientId { get; set; }


        public UserPatient ToModel()
        {
            return new UserPatient
            {
                UserId = UserId,
                PatientId = PatientId,
            };
        }
    }
}
