
using backend.models;



namespace backend.repository
{
    public interface IUserRepository
    {
        
        Task AddUser(UserModel user);

        Task<UserModel?> GetUserById(int id);
        Task RemoveUser(int id);
        Task UpdateUser(UserModel user);
    }


}
