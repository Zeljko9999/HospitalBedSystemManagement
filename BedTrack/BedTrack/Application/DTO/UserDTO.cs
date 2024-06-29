using BedTrack.Domain.Models;

namespace BedTrack.Application.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }

        public string? Status { get; set; }

        public string Clinic { get; set; }

        public string Department { get; set; }

        public static UserDTO FromModel(User model)
        {
            return new UserDTO
            {
                Id = model.Id,
                Name = model.Name,
                Role = model.Role,
                Email = model.Email,
                Status = model.Status,
                Clinic = model.Clicnic.Name,
                Department = model.Department.Name,
            };
        }
    }

}
