using backend.Models.Domain;

namespace backend.Repositories
{
    public interface IUserQueueRepository
    {
        Task<IEnumerable<UserQueue>> GetAllUserQueue();

        Task<IEnumerable<UserQueue>> GetUserQueueByBookId(int bookId);

        Task<UserQueue?> GetUserQueue(int bookId, int userId);

        Task CreateUserQueue(UserQueue userQueue);
        Task DeleteUserQueue(UserQueue userQueue);
    }
}