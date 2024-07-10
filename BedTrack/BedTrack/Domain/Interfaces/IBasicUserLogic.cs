using BedTrack.Domain.Models;
using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;

namespace BedTrack.Domain.Interfaces
{
    public interface IBasicUserLogic
    {
        Task<IEnumerable<UserDTO>> GetUsers();

        Task<UserDTO> GetUser(int id);

        Task<UserDTO> GetUserByEmail(string emial);

        Task UpdateUser(int id, NewUserDTO? updatedUser);

        Task DeleteUser(int id);

        Task<IEnumerable<string>> GetUsersByClinicId(int clinicId);

        Task<IEnumerable<string>> GetUsersByDepartmentId(int departmentId);
    }
}