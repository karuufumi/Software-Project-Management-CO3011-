using backend.models;
using Microsoft.EntityFrameworkCore;
using backend.repository;
using backend.Data;

namespace backend.repository
{
    public class QueueRepository : IQueueRepository
    {
        private readonly ApplicationDbContext _context;

        public QueueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddToQueue(BookModel book, int userId)
        {
            return _context.Queues
                .Where(q => q.UserId == userId)
                .ForEachAsync(q => q.bookList.Add(book));
        } 

        
        public Task<Queue?> GetQueueByBookId(int bookId, int userId)
        {
            return _context.Queues
                .Include(q => q.bookList)
                .FirstOrDefaultAsync(q => q.bookList.Any(b => b.BookId == bookId) && q.UserId == userId);
        }

        
        public Task<Queue?> GetQueueByUserId(int userId)
        {
            return _context.Queues
                .Include(q => q.bookList)
                .FirstOrDefaultAsync(q => q.UserId == userId);
        }


        public Task IsQueueEmpty(int userId)
        {
            return _context.Queues
                .Include(q => q.bookList)
                .Where(q => q.UserId == userId && !q.bookList.Any())
                .FirstOrDefaultAsync();
        }

        public Task Pop(int userId)
        {
            return _context.Queues
                .Where(q => q.UserId == userId)
                .ForEachAsync(q => 
                {
                    if (q.bookList.Any())
                    {
                        q.bookList.RemoveAt(0); // Remove the first book in the queue
                    }
                });
        }


        public Task<BookModel?> Peek(int userId)
        {
            return _context.Queues
                .Where(q => q.UserId == userId)
                .SelectMany(q => q.bookList)
                .FirstOrDefaultAsync();
        }

        
    }
}