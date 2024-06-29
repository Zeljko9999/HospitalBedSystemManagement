using BedTrack.Domain.Models;
using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;

namespace BedTrack.Domain.Interfaces
{
    public interface IUserLogic
    {
        Task CreateNewUser(NewUserDTO? user);

        Task<IEnumerable<UserDTO>> GetUsers();

        Task<UserDTO> GetUser(int id);

        Task UpdateUser(int id, NewUserDTO? updatedUser);

        Task DeleteUser(int id);

        Task<IEnumerable<string>> GetUsersByClinicId(int clinicId);

        Task<IEnumerable<string>> GetUsersByDepartmentId(int departmentId);
    }
}
