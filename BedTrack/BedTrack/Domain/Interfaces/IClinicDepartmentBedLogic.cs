using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;

namespace BedTrack.Domain.Interfaces
{
    public interface IClinicDepartmentBedLogic
    {
        Task CreateNewClinicDepartmentBed(NewClinicDepartmentBedDTO? clinicDepartmentBed);

        Task<ClinicDepartmentBedDTO> GetClinicDepartmentBedRow(int id);

        Task UpdateClinicDepartmentBed(int id, NewClinicDepartmentBedDTO? updatedClinicDepartmentBed);

        Task DeleteClinicDepartmentBed(int id);
    }
}
