using BedTrack.Domain.Models;

namespace BedTrack.Application.DTO
{
    public class UserPatientDTO
    {
        public int Id { get; set; }
        public string? User { get; set; }
        public int UserId { get; set; }

        public string? Patient { get; set; }
        public int PatientId { get; set; }

        public static UserPatientDTO FromModel(UserPatient model)
        {
            return new UserPatientDTO
            {
                Id = model.Id,
                User = model.User.Name,
                UserId = model.UserId,
                Patient = model.Patient.Name,
                PatientId = model.PatientId,
            };
        }
    }
}
