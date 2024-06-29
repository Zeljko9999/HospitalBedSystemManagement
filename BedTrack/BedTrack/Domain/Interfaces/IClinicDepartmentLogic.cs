using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;

namespace BedTrack.Domain.Interfaces
{
    public interface IClinicDepartmentLogic
    {
        Task CreateNewClinicDepartment(NewClinicDepartmentDTO? clinicDepartment);

        Task<IEnumerable<ClinicDepartmentDTO>> GetClinicDepartments(int clinicId);

        Task<ClinicDepartmentDTO> GetClinicDepartment(int clinicId, int departmentId);
        Task<ClinicDepartmentDTO> GetClinicDepartmentRow(int id);

        Task UpdateClinicDepartment(int id, NewClinicDepartmentDTO? updatedClinicDepartment);

        Task DeleteClinicDepartment(int id);
    }
}
