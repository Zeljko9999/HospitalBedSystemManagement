using BedTrack.Domain.Models;

namespace BedTrack.DAL.Interfaces
{
    public interface IUserPatientRepository
    {
        Task AddUserPatient(UserPatient? userPatient);

        Task<UserPatient> GetUserPatient(int id);

        Task<UserPatient> GetPatientForUser(int userId, int patientId);

        Task UpdateUserPatient(UserPatient? updatedUserPatient);

        Task DeleteUserPatient(int id);

        Task<IEnumerable<UserPatient>> GetPatientsForUser(int userId);

        Task<IEnumerable<UserPatient>> GetUsersForPatient(int patientId);
    }
}
