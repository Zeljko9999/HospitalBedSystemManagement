using BedTrack.DAL.Interfaces;
using BedTrack.Configuration;
using BedTrack.Domain.Models;
using BedTrack.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace BedTrack.DAL.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly BedTrackContext db;


        public PatientRepository(BedTrackContext context)
        {
            db = context;
        }

        public async Task AddPatient(Patient? patient)
        {
            db.Patients.Add(patient);
            await db.SaveChangesAsync();
        }

        public async Task UpdatePatient(Patient? updatedPatient)
        {
            db.Patients.Update(updatedPatient);
            await db.SaveChangesAsync();
        }

        public async Task DeletePatient(int id)

        {
            var patient = await db.Patients.FindAsync(id);
            if (patient != null)
            {
                db.Patients.Remove(patient);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Patient> GetPatient(int id)
        {
            try
            {
                return await db.Patients
                        .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving patient.", ex);
            }
        }
    }
}
