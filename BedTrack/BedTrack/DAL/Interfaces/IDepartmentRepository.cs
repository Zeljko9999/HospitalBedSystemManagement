using BedTrack.Domain.Models;

namespace BedTrack.DAL.Interfaces
{
    public interface IDepartmentRepository
    {
        Task AddDepartment(Department? department);

        Task<List<Department>> GetAllDepartments();

        Task<Department> GetDepartment(int id);

        Task UpdateDepartment(Department? updatedDepartment);

        Task DeleteDepartment(int id);
    }
}