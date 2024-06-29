using BedTrack.DAL.Interfaces;
using BedTrack.Configuration;
using BedTrack.Domain.Models;
using BedTrack.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace BedTrack.DAL.Repositories
{
    public class BedRepository : IBedRepository
    {
        private readonly BedTrackContext db;

        public BedRepository(BedTrackContext context)
        {
            db = context;
        }

        public async Task AddBed(Bed? bed)
        {
            db.Beds.Add(bed);
            await db.SaveChangesAsync();
        }

        public async Task UpdateBed(Bed? updatedBed)
        {
            db.Beds.Update(updatedBed);
            await db.SaveChangesAsync();
        }

        public async Task DeleteBed(int id)

        {
            var bed = await db.Beds.FindAsync(id);
            if (bed != null)
            {
                db.Beds.Remove(bed);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Bed> GetBed(int id)
        {
            try
            {
                return await db.Beds
                            .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving bed.", ex);
            }
        }

        public async Task<List<Bed>> GetAllBeds()
        {
            try
            {
                return await db.Beds
                             .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving bed.", ex);
            }
        }
    }
}
