using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;
using BedTrack.Domain.Models;

namespace BedTrack.Domain.Interfaces
{
    public interface IClinicDepartmentBedLogic
    {
        Task CreateNewClinicDepartmentBed(NewClinicDepartmentBedDTO? clinicDepartmentBed);

        Task<ClinicDepartmentBedDTO> GetClinicDepartmentBedRow(int id);

        Task<IEnumerable<ClinicDepartmentBedDTO>> GetBedsForClinicDepartment(int clinicId, int departmentId); 

        Task UpdateClinicDepartmentBed(int id, NewClinicDepartmentBedDTO? updatedClinicDepartmentBed);

        Task DeleteClinicDepartmentBed(int id);
    }
}
