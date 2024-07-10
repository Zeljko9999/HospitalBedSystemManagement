using BedTrack.Domain.Models;

namespace BedTrack.DAL.Interfaces
{
    public interface IPatientRepository
    {
        Task AddPatient(Patient? patient);
        Task UpdatePatient(Patient? updatedPatient);
        Task DeletePatient(int id);
        Task<Patient> GetPatient(int id);
        Task<List<Patient>> GetPatientsWithoutBeds();
    }
}
