using backend.models;

namespace backend.repository
{
    public interface IQueueRepository
    {
        // Add a book to user's queue
        Task Push(string queueId, string bookId);
        
        // Remove a specific book from queue
        Task Pop(string queueId, string bookId);
        
        // Get user's queue
        Task<BookQueue?> GetQueueById(string userId);
        
        // Create queue for new user
        Task CreateQueue(string userId);
        
        // Check if queue is empty
        Task<bool> IsEmpty(string queueId);
        
        // Clear entire queue
        Task ClearQueue(string queueId);
        
        // Get all books in queue
        Task<IEnumerable<BookModel>> GetQueueBooks(string queueId);
    }
}