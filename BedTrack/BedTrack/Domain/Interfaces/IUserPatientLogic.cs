using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;
using BedTrack.Domain.Models;

namespace BedTrack.Domain.Interfaces
{
    public interface IUserPatientLogic
    {
        Task CreateNewUserPatient(NewUserPatientDTO? userPatient);

        Task<UserPatientDTO> GetUserPatientRow(int id);

        Task UpdateUserPatient(int id, NewUserPatientDTO? updatedUserPatient);

        Task DeleteUserPatient(int id);

        Task<IEnumerable<UserPatientDTO>> GetPatientsForUser(int userId);

        Task<IEnumerable<UserPatientDTO>> GetUsersForPatient(int patientId);
    }
}
