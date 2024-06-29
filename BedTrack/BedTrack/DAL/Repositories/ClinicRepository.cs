using BedTrack.DAL.Interfaces;
using BedTrack.Configuration;
using BedTrack.Domain.Models;
using BedTrack.DAL.Data;
using Microsoft.EntityFrameworkCore;
namespace BedTrack.DAL.Repositories
{
    public class ClinicRepository : IClinicRepository
    {
        private readonly BedTrackContext db;

        public ClinicRepository(BedTrackContext context)
        {
            db = context;
        }


        public async Task AddClinic(Clinic? clinic)
        {
            db.Clinics.Add(clinic);
            await db.SaveChangesAsync();
        }

        public async Task UpdateClinic(Clinic? updatedClinic)
        {
            db.Clinics.Update(updatedClinic);
            await db.SaveChangesAsync();
        }

        public async Task DeleteClinic(int id)

        {
            var clinic = await db.Clinics.FindAsync(id);
            if (clinic != null)
            {
                db.Clinics.Remove(clinic);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Clinic> GetClinic(int id)
        {
            try
            {
                return await db.Clinics
                            .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving clinic.", ex);
            }
        }

        public async Task<List<Clinic>> GetAllClinics()
        {
            try
            {
                return await db.Clinics
                             .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving clinics.", ex);
            }
        }
    }
}
