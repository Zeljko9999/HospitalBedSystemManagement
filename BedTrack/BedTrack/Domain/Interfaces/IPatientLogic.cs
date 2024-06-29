using BedTrack.Application.NewDTO;
using BedTrack.Application.DTO;
using BedTrack.Domain.Models;

namespace BedTrack.Domain.Interfaces
{
    public interface IPatientLogic
    {
        Task CreateNewPatient(NewPatientDTO? patient);
        Task UpdatePatient(int id, NewPatientDTO? updatedPatient);
        Task DeletePatient(int id);
        Task<PatientDTO> GetPatient(int id);
    }
}
