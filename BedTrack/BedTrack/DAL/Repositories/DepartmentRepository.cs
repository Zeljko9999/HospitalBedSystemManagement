using BedTrack.DAL.Interfaces;
using BedTrack.Configuration;
using BedTrack.Domain.Models;
using BedTrack.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace BedTrack.DAL.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly BedTrackContext db;

        public DepartmentRepository(BedTrackContext context)
        {
            db = context;
        }

        public async Task AddDepartment(Department? department)
        {
            db.Departments.Add(department);
            await db.SaveChangesAsync();
        }

        public async Task UpdateDepartment(Department? updatedDepartment)
        {
            db.Departments.Update(updatedDepartment);
            await db.SaveChangesAsync();
        }

        public async Task DeleteDepartment(int id)

        {
            var department = await db.Departments.FindAsync(id);
            if (department != null)
            {
                db.Departments.Remove(department);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Department> GetDepartment(int id)
        {
            try
            {
                return await db.Departments
                            .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving department.", ex);
            }
        }

        public async Task<List<Department>> GetAllDepartments()
        {
            try
            {
                return await db.Departments
                             .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving department.", ex);
            }
        }
    }
}
