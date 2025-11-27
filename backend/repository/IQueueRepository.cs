using backend.models;
using Microsoft.EntityFrameworkCore;
using backend.repository;



namespace backend.repository
{
    
    public interface IQueueRepository
    {
        // Define methods for managing the Queue here
        //Task AddToQueue(Queue queueEntry);

        Task Push(BookModel newbook, string queueId);
        Task<BookQueue?> GetQueueById(string id);

        Task<bool> IsEmpty(string id);


        Task Pop(string id);
        Task UpdateQueue(string queueId, BookModel updatedBook);
        Task ClearQueue(string id);
        
    }
    
}