using backend.models;

namespace backend.Repositories
{
    public interface IQueueRepository
    {
        Task AddAsync(BorrowRequest item);
        Task UpdateAsync(BorrowRequest item);
        Task<BorrowRequest?> GetNextPersonWaitingAsync(int bookId);
        Task<bool> IsUserAlreadyInQueueAsync(int userId, int bookId);
        Task<int> GetQueuePositionAsync(int userId, int bookId);
        Task<List<BorrowRequest>> GetExpiredAllocationsAsync();
    }
}