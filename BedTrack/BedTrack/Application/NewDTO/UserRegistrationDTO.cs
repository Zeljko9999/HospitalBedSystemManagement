using BedTrack.Domain.Models;

namespace BedTrack.Application.NewDTO
{
    public class UserRegistrationDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string? Status { get; set; }
        public int ClinicId { get; set; }
        public int DepartmentId { get; set; }
        public string Role { get; set; }

        public User ToModel()
        {
            return new User
            {
                UserName = Email,
                Name = Name,
                Email = Email,
                Status = Status,
                ClinicId = ClinicId,
                DepartmentId = DepartmentId
            };
        }
    }
}
