using BedTrack.DAL.Interfaces;
using BedTrack.Configuration;
using BedTrack.Domain.Models;
using BedTrack.DAL.Data;
using Microsoft.EntityFrameworkCore;
using BedTrack.Application.DTO;

namespace BedTrack.DAL.Repositories
{
    public class ClinicDepartmentBedRepository : IClinicDepartmentBedRepository
    {
        private readonly BedTrackContext db;


        public ClinicDepartmentBedRepository(BedTrackContext context)
        {
            db = context;
        }

        public async Task AddClinicDepartmentBed(ClinicDepartmentBed? clinicDepartmentBed)
        {
            db.ClinicDepartmentBeds.Add(clinicDepartmentBed);
            await db.SaveChangesAsync();
        }

        public async Task UpdateClinicDepartmentBed(ClinicDepartmentBed? updatedClinicDepartmentBed)
        {
            db.ClinicDepartmentBeds.Update(updatedClinicDepartmentBed);
            await db.SaveChangesAsync();
        }

        public async Task DeleteClinicDepartmentBed(int id)

        {
            var clinicDepartmentBed = await db.ClinicDepartmentBeds.FindAsync(id);
            if (clinicDepartmentBed != null)
            {
                db.ClinicDepartmentBeds.Remove(clinicDepartmentBed);
                await db.SaveChangesAsync();
            }
        }

        public async Task<ClinicDepartmentBed> GetClinicDepartmentForBed(int clinicDepartmentId, int bedId)
        {
            try
            {
                return await db.ClinicDepartmentBeds
                        .FirstOrDefaultAsync(u => u.ClinicDepartmentId == clinicDepartmentId && u.BedId == bedId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving clinicDepartmentBed.", ex);
            }
        }

        public async Task<List<ClinicDepartmentBed>> GetAllBedsOfClinicDepartment(int clinicDepartmentId)
        {
            try
            {
                return await db.ClinicDepartmentBeds
                             .Include(cdb => cdb.ClinicDepartment)
                                .ThenInclude(cd => cd.Clinic)
                             .Include(cdb => cdb.ClinicDepartment)
                                .ThenInclude(cd => cd.Department)
                             .Include(cdb => cdb.Bed)
                             .Include(cdb => cdb.Patient)
                             .Where(u => u.ClinicDepartmentId == clinicDepartmentId)
                             .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving clinicDepartments.", ex);
            }
        }

        public async Task<ClinicDepartmentBed> GetClinicDepartmentForPatient(int patientId)
        {
            try
            {
                return await db.ClinicDepartmentBeds
                        .FirstOrDefaultAsync(u => u.PatientId == patientId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving clinicDepartmentBedPatient.", ex);
            }
        }

        public async Task<ClinicDepartmentBed> GetClinicDepartmentBed(int id)
        {
            try
            {
                var clinicDepartmentBed = await db.ClinicDepartmentBeds
                    .Include(cdb => cdb.ClinicDepartment)
                        .ThenInclude(cd => cd.Clinic)
                    .Include(cdb => cdb.ClinicDepartment)
                        .ThenInclude(cd => cd.Department)
                    .Include(cdb => cdb.Bed)
                    .Include(cdb => cdb.Patient)
                    .FirstOrDefaultAsync(cdb => cdb.Id == id);

                if (clinicDepartmentBed == null)
                {
                    throw new KeyNotFoundException("ClinicDepartmentBed not found.");
                }

                return clinicDepartmentBed;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving clinicDepartmentBed.", ex);
            }
        }
    }
}
