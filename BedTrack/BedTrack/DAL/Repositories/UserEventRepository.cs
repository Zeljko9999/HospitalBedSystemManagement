using BedTrack.DAL.Interfaces;
using BedTrack.Configuration;
using BedTrack.Domain.Models;
using BedTrack.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace BedTrack.DAL.Repositories
{
    public class UserEventRepository : IUserEventRepository
    {
        private readonly BedTrackContext db;


        public UserEventRepository(BedTrackContext context)
        {
            db = context;
        }


        public async Task AddUserEvent(UserEvent? userEvent)
        {
            db.UserEvents.Add(userEvent);
            await db.SaveChangesAsync();
        }

        public async Task UpdateUserEvent(UserEvent? updatedUserEvent)
        {
            db.UserEvents.Update(updatedUserEvent);
            await db.SaveChangesAsync();
        }

        public async Task DeleteUserEvent(int id)

        {
            var userEvent = await db.UserEvents.FindAsync(id);
            if (userEvent != null)
            {
                db.UserEvents.Remove(userEvent);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Event> GetEventForUser(int userId)
        {
            try
            {
                DateTime now = DateTime.Now;

                var closestEvent = await (from ue in db.UserEvents
                                          join e in db.Events on ue.EventId equals e.Id
                                          where ue.UserId == userId
                                          orderby Math.Abs(EF.Functions.DateDiffSecond(e.Alarm, now))
                                          select e).FirstOrDefaultAsync();

                return closestEvent;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving userEvent.", ex);
            }
        }

        public async Task<List<Event>> GetAllEventsOfUser(int userId)
        {
            try
            {
                return await (from ue in db.UserEvents
                                    join e in db.Events on ue.EventId equals e.Id
                                    where ue.UserId == userId
                                    select e).ToListAsync();

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving UserEvents.", ex);
            }
        }

        public async Task<UserEvent> GetUserEvent(int id)
        {
            try
            {
                return await db.UserEvents
                        .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving UserEvent.", ex);
            }
        }
    }
}
