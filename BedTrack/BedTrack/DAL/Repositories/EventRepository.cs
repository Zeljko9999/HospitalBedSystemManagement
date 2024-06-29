using BedTrack.DAL.Data;
using BedTrack.DAL.Interfaces;
using BedTrack.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BedTrack.DAL.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly BedTrackContext db;

        public EventRepository(BedTrackContext context)
        {
            db = context;
        }


        public async Task AddEvent(Event? eventt)
        {
            db.Events.Add(eventt);
            await db.SaveChangesAsync();
        }

        public async Task UpdateEvent(Event? updatedEvent)
        {
            db.Events.Update(updatedEvent);
            await db.SaveChangesAsync();
        }

        public async Task DeleteEvent(int id)

        {
            var eventt = await db.Events.FindAsync(id);
            if (eventt != null)
            {
                db.Events.Remove(eventt);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Event> GetEvent(int id)
        {
            try
            {
                return await db.Events
                            .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving event.", ex);
            }
        }
        
    }
}
