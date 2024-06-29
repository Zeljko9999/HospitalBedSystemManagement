using BedTrack.DAL.Interfaces;
using BedTrack.Configuration;
using BedTrack.Domain.Models;
using BedTrack.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace BedTrack.DAL.Repositories
{
    public class UserPatientRepository : IUserPatientRepository
    {
        private readonly BedTrackContext db;

        public UserPatientRepository(BedTrackContext context)
        {
            db = context;
        }

        public async Task AddUserPatient(UserPatient? userPatient)
        {
            db.UserPatients.Add(userPatient);
            await db.SaveChangesAsync();
        }

        public async Task UpdateUserPatient(UserPatient? updatedUserPatient)
        {
            db.UserPatients.Update(updatedUserPatient);
            await db.SaveChangesAsync();
        }

        public async Task DeleteUserPatient(int id)

        {
            var userPatient = await db.UserPatients.FindAsync(id);
            if (userPatient != null)
            {
                db.UserPatients.Remove(userPatient);
                await db.SaveChangesAsync();
            }
        }

        public async Task<UserPatient> GetUserPatient(int id)
        {
            try
            {
                return await db.UserPatients
                            .Include(u => u.User)
                            .Include(u => u.Patient)
                            .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving userPatient.", ex);
            }
        }

        public async Task<UserPatient> GetPatientForUser(int userId, int patientId)
        {
            try
            {
                return await db.UserPatients
                        .Include(u => u.User)
                        .Include(u => u.Patient)
                        .FirstOrDefaultAsync(u => u.UserId == userId && u.PatientId == patientId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving UserPatient.", ex);
            }
        }

        public async Task<IEnumerable<UserPatient>> GetPatientsForUser(int userId)
        {
            try
            {
                return await db.UserPatients
                            .Where(u => u.UserId == userId)
                            .Include(u => u.Patient)
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving patients for user.", ex);
            }
        }

        public async Task<IEnumerable<UserPatient>> GetUsersForPatient(int patientId)
        {
            try
            {
                return await db.UserPatients
                            .Where(u => u.PatientId == patientId)
                            .Include(u => u.User)
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving useres for patient.", ex);
            }
        }
    }
}
