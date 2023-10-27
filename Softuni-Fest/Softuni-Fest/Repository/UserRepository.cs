using Microsoft.EntityFrameworkCore;
using Softuni_Fest.Interfaces;

namespace Softuni_Fest.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _Context;
        public UserRepository(ApplicationDbContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            _Context.Add(user);
            return await SaveAsync();
        }

        public async Task<ICollection<User>> GetRecommendedUser(string term)
        {
            return await _Context.Users.Where(x => x.NormalizedUserName.Contains(term.ToUpper())).ToListAsync();
        }

        public async Task<User> GetUserAsync(string id)
        {
            return await _Context.Users.FindAsync(id);
        }

        public async Task<ICollection<User>> GetUsersAsync()
        {
            return await _Context.Users.ToListAsync();
        }

        public async Task<bool> RemoveUserAsync(User user)
        {
            _Context.Remove(user);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _Context.Update(user);
            return await SaveAsync();
        }

        public async Task<bool> UserExistsAsync(string id)
        {
            return await _Context.Users.AnyAsync(x => x.Id == id);
        }
    }
}
