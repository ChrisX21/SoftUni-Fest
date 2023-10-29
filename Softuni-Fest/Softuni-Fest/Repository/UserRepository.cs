using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Softuni_Fest.Interfaces;
using Softuni_Fest.Models;

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
            List<User> users = await _Context.Users.ToListAsync();

            Dictionary<User, int> userSimilarity = new Dictionary<User, int>();

            foreach (User user in users)
            {
                int similarity = Calculate(term, user.UserName);
                Console.WriteLine(similarity + " term " + user.UserName);
                userSimilarity[user] = similarity;
            }

            List<User> sortedUsers = userSimilarity.OrderBy(x => x.Value)
                .Select(x => x.Key)
                .Take(3)
                .ToList();

            return sortedUsers;
        }

        private static int Calculate(string source1, string source2)
        {
            var source1Length = source1.Length;
            var source2Length = source2.Length;

            var matrix = new int[source1Length + 1, source2Length + 1];

            if (source1Length == 0)
                return source2Length;

            if (source2Length == 0)
                return source1Length;

            for (var i = 0; i <= source1Length; matrix[i, 0] = i++) { }
            for (var j = 0; j <= source2Length; matrix[0, j] = j++) { }

            for (var i = 1; i <= source1Length; i++)
            {
                for (var j = 1; j <= source2Length; j++)
                {
                    var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }

            return matrix[source1Length, source2Length];
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

        public async Task<List<User>> GetUsersFromSearchTerm(string searchTerm)
        {
            return await _Context.Users.Where((x) => x.NamePersonal.ToLower().Contains(searchTerm.ToLower())).ToListAsync();
        }
    }
}
