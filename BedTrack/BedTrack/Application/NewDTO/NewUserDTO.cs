using BedTrack.Domain.Models;

namespace BedTrack.Application.NewDTO
{
    public class NewUserDTO
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Status { get; set; }
        public int ClinicId { get; set; }
        public int DepartmentId { get; set; }

        public User ToModel()
        {
            return new User
            {
                Name = Name,
                Role = Role,
                Email = Email,
                Password = Password,
                Status = Status,
                ClinicId = ClinicId,
                DepartmentId = DepartmentId,
                AlarmEvents = null,
                Patients = null
            };
        }
    }
}
