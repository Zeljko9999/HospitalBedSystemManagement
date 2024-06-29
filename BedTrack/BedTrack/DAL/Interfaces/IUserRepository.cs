using BedTrack.Domain.Models;

namespace BedTrack.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task AddUser(User? user);

        Task<List<User>> GetAllUsers();

        public Task<User> GetUser(int id);

        Task UpdateUser(User? updatedUser);

        Task DeleteUser(int id);

        Task<IEnumerable<string>> GetUsersByClinicId(int clinicId);

        Task<IEnumerable<string>> GetUsersByDepartmentId(int departmentId);
    }
}