using BedTrack.Domain.Models;

namespace BedTrack.DAL.Interfaces
{
    public interface IClinicDepartmentRepository
    {
        Task AddClinicDepartment(ClinicDepartment? clinicDepartment);

        Task<List<ClinicDepartment>> GetAllDepartmentsOfClinic(int clinicId);

        public Task<ClinicDepartment> GetDepartmentForClinic(int clinicId, int departmentId);
        public Task<ClinicDepartment> GetClinicDepartment(int id);

        Task UpdateClinicDepartment(ClinicDepartment? updatedClinicDepartment);

        Task DeleteClinicDepartment(int id);
    }
}
