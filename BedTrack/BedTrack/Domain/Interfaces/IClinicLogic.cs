using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;


namespace BedTrack.Domain.Interfaces
{
    public interface IClinicLogic
    {
        Task CreateNewClinic(NewClinicDTO? clinic);

        Task<IEnumerable<ClinicDTO>> GetClinics();

        Task<ClinicDTO> GetClinic(int id);

        Task UpdateClinic(int id, NewClinicDTO? updatedClinic);

        Task DeleteClinic(int id);
    }
}
