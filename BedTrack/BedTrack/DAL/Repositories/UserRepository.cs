using BedTrack.DAL.Interfaces;
using BedTrack.Configuration;
using BedTrack.Domain.Models;
using BedTrack.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace BedTrack.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BedTrackContext db;
        private readonly UserManager<User> _userManager;

        public UserRepository(BedTrackContext context, UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;
        }


        public async Task UpdateUser(User? updatedUser)
        {
            db.Users.Update(updatedUser);
            await db.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await db.Users.FindAsync(id);
            if (user != null)
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
            }
        }

        public async Task<User> GetUser(int id)
        {
            try
            {
                return await db.Users
                        .Include(u => u.Clicnic)
                        .Include(u => u.Department)
                        .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving user.", ex);
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                return await db.Users
                        .Include(u => u.Clicnic)
                        .Include(u => u.Department)
                        .FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving user.", ex);
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return await db.Users
                             .Include(u => u.Clicnic)
                             .Include(u => u.Department)
                             .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving users.", ex);
            }
        }

        public async Task<IEnumerable<string>> GetUsersByClinicId(int clinicId)
        {
            return await db.Users
                 .Where(u => u.ClinicId == clinicId)
                 .Select(u => u.Name)
                 .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetUsersByDepartmentId(int departmentId)
        {
            return await db.Users
                 .Where(u => u.DepartmentId == departmentId)
                 .Select(u => u.Name)
                 .ToListAsync();
        }

        public async Task<string> GetUserRole(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }
    }
}
