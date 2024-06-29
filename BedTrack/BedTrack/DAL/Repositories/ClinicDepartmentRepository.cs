using BedTrack.DAL.Interfaces;
using BedTrack.Configuration;
using BedTrack.Domain.Models;
using BedTrack.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace BedTrack.DAL.Repositories
{
    public class ClinicDepartmentRepository : IClinicDepartmentRepository
    {
        private readonly BedTrackContext db;


        public ClinicDepartmentRepository(BedTrackContext context)
        {
            db = context;
        }


        public async Task AddClinicDepartment(ClinicDepartment? clinicDepartment)
        {
            db.ClinicDepartments.Add(clinicDepartment);
            await db.SaveChangesAsync();
        }

        public async Task UpdateClinicDepartment(ClinicDepartment? updatedClinicDepartment)
        {
            db.ClinicDepartments.Update(updatedClinicDepartment);
            await db.SaveChangesAsync();
        }

        public async Task DeleteClinicDepartment(int id)

        {
            var clinicDepartment = await db.ClinicDepartments.FindAsync(id);
            if (clinicDepartment != null)
            {
                db.ClinicDepartments.Remove(clinicDepartment);
                await db.SaveChangesAsync();
            }
        }

        public async Task<ClinicDepartment> GetDepartmentForClinic(int clinicId, int departmentId)
        {
            try
            {
                return await db.ClinicDepartments
                        .Include(u => u.Clinic)
                        .Include(u => u.Department)
                        .FirstOrDefaultAsync(u => u.ClinicId == clinicId && u.DepartmentId == departmentId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving clinicDepartment.", ex);
            }
        }

        public async Task<List<ClinicDepartment>> GetAllDepartmentsOfClinic(int clinicId)
        {
            try
            {
                return await db.ClinicDepartments
                             .Include(u => u.Clinic)
                             .Include(u => u.Department)
                             .Where(u => u.ClinicId == clinicId)
                             .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving clinicDepartments.", ex);
            }
        }

        public async Task<ClinicDepartment> GetClinicDepartment(int id)
        {
            try
            {
                return await db.ClinicDepartments
                        .Include(u => u.Clinic)
                        .Include(u => u.Department)
                        .FirstOrDefaultAsync(u => u.Id == id );
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving clinicDepartment.", ex);
            }
        }
    }
}
