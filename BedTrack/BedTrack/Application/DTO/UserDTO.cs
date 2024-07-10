using BedTrack.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace BedTrack.Application.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public string? Status { get; set; }

        public int ClinicId { get; set; }

        public string Clinic { get; set; }

        public int DepartmentId { get; set; }

        public string Department { get; set; }

        public static UserDTO FromModel(User model, string role)
        {
            return new UserDTO
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                Role = role,
                Status = model.Status,
                ClinicId = model.ClinicId,
                Clinic = model.Clicnic.Name,
                DepartmentId = model.DepartmentId,
                Department = model.Department.Name,
            };
        }
    }

}
