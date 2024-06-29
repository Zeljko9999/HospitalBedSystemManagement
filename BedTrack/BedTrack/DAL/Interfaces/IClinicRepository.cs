using BedTrack.Domain.Models;

namespace BedTrack.DAL.Interfaces
{
    public interface IClinicRepository
    {
        Task AddClinic(Clinic? clinic);

        Task<List<Clinic>> GetAllClinics();

        public Task<Clinic> GetClinic(int id);

        Task UpdateClinic(Clinic? updatedClinic);

        Task DeleteClinic(int id);
    }
}
