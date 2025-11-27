using backend.models;
using Microsoft.EntityFrameworkCore;
using backend.repository;



namespace backend.repository
{
    
    public interface IQueueRepository
    {
        // Define methods for managing the Queue here
        //Task AddToQueue(Queue queueEntry);

        Task AddToQueue(BookModel book, int userId);
        //Task<Queue?> GetQueueByUserId(int userId);
        Task Pop(int userId);

        Task<BookModel?> Peek(int userId);

        //Task UpdateQueueEntry(Queue queueEntry);

        
        Task<Queue?> GetQueueByBookId(int bookId, int userId);

        Task<Queue?> GetQueueByUserId(int userId);
        Task IsQueueEmpty(int userId);

        
    }
    
}