using backend.models;

namespace backend.repository
{
    public interface IUserRepository<T> where T : UserModel
    {
        Task AddUser(T user);
        Task<T?> GetUserById(string id);
        Task<int> GetUserMembership(string userId);
                Task<IEnumerable<T>> GetAllUsers();
        Task RemoveUser(string id);
        Task UpdateUser(T user);
        Task<T?> UpdateCredit(string userId, int credits);
    }
}
