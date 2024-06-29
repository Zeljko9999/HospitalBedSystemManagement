using BedTrack.Domain.Models;
using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;

namespace BedTrack.Domain.Interfaces
{
    public interface IDepartmentLogic
    {
        Task CreateNewDepartment(NewDepartmentDTO? department);

        Task<IEnumerable<DepartmentDTO>> GetDepartments();

        Task<DepartmentDTO> GetDepartment(int id);

        Task UpdateDepartment(int id, NewDepartmentDTO? updatedDepartment);

        Task DeleteDepartment(int id);
    }
}
