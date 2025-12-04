using backend.Data;
using backend.models;
using Microsoft.EntityFrameworkCore;

namespace backend.repository
{
    public class QueueRepository : IQueueRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookRepository _bookRepository;

        public QueueRepository(ApplicationDbContext context, IBookRepository bookRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
        }

        public async Task Push(string queueId, string bookId)
        {
            var queue = await GetQueueById(queueId);
            if (queue == null)
            {
                throw new InvalidOperationException("Queue not found.");
            }

            var book = await _bookRepository.GetBookById(bookId);
            if (book == null)
            {
                throw new InvalidOperationException("Book not found.");
            }

            // Check if book is already in queue
            if (queue.QueuedBooks?.Any(b => b.BookId == bookId) == true)
            {
                throw new InvalidOperationException("Book is already in queue.");
            }

            if (queue.QueuedBooks == null)
            {
                queue.QueuedBooks = new List<BookModel>();
            }

            queue.QueuedBooks.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task Pop(string queueId, string bookId)
        {
            var queue = await GetQueueById(queueId);
            if (queue == null)
            {
                throw new InvalidOperationException("Queue not found.");
            }

            var book = queue.QueuedBooks?.FirstOrDefault(b => b.BookId == bookId);
            if (book == null)
            {
                throw new InvalidOperationException("Book not found in queue.");
            }

            queue.QueuedBooks?.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<BookQueue?> GetQueueById(string userId)
        {
            return await _context.BookQueues
                .Include(q => q.QueuedBooks)
                .FirstOrDefaultAsync(q => q.UserId == userId);
        }

        public async Task CreateQueue(string userId)
        {
            var existingQueue = await GetQueueById(userId);
            if (existingQueue != null)
            {
                throw new InvalidOperationException("Queue already exists for this user.");
            }

            var newQueue = new BookQueue
            {
                UserId = userId,
                QueuedBooks = new List<BookModel>()
            };

            await _context.BookQueues.AddAsync(newQueue);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsEmpty(string queueId)
        {
            var queue = await GetQueueById(queueId);
            return queue?.QueuedBooks?.Count == 0 || queue?.QueuedBooks == null;
        }

        public async Task ClearQueue(string queueId)
        {
            var queue = await GetQueueById(queueId);
            if (queue == null)
            {
                throw new InvalidOperationException("Queue not found.");
            }

            queue.QueuedBooks?.Clear();
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookModel>> GetQueueBooks(string queueId)
        {
            var queue = await GetQueueById(queueId);
            return queue?.QueuedBooks ?? Enumerable.Empty<BookModel>();
        }
    }
}