using BedTrack.Domain.Models;
using System.Threading.Tasks;

namespace BedTrack.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();

        public Task<User> GetUser(int id);

        public Task<User> GetUserByEmail(string email);

        Task UpdateUser(User? updatedUser);

        Task DeleteUser(int id);

        Task<string> GetUserRole(User user);

        Task<IEnumerable<string>> GetUsersByClinicId(int clinicId);

        Task<IEnumerable<string>> GetUsersByDepartmentId(int departmentId);
    }
}