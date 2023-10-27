namespace Softuni_Fest.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(User user);
        Task<bool> RemoveUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> UserExistsAsync(string id);
        Task<bool> SaveAsync();
        Task<User> GetUserAsync(string id);
        Task<ICollection<User>> GetUsersAsync();
        Task<ICollection<User>> GetRecommendedUser(string term);
    }
}
