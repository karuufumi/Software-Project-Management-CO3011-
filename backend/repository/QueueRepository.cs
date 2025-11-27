using backend.models;
using Microsoft.EntityFrameworkCore;
using backend.repository;
using backend.Data;

namespace backend.repository
{
    
    public class QueueRepository : IQueueRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<BookQueue> _bookQueues;

        public QueueRepository(ApplicationDbContext context)
        {
            _context = context;
            _bookQueues = _context.Set<BookQueue>();
        }

        public async Task Push(BookModel newbook, string queueId)
        {
            var queue = await _bookQueues.FindAsync(queueId);
            if (queue != null)
            {
                queue.bookList.Add(newbook);
                _bookQueues.Update(queue);
                await _context.SaveChangesAsync();
            }
            Console.WriteLine("Queue not found.");
        }
       
        public async Task<BookQueue?> GetQueueById(string id)
        {
            return await _bookQueues.FindAsync(id);
        }

        public async Task<bool> IsEmpty(string id)
        {
            BookQueue? queue = await _bookQueues.FindAsync(id);
            if (queue != null)
            {
               if (queue.bookList.Count == 0)
               {
                    Console.WriteLine("The queue is empty.");
                    return true;
               }
               else
               {
                    Console.WriteLine("The queue is not empty.");
                    return false;
               }
            }
            else
            {
                Console.WriteLine("Queue not found.");
                return false;
            }
        }

        public async Task Pop(string id)
        {
            if (_bookQueues.Find(id) is BookQueue queue)
            {
                if (queue.bookList.Count > 0)
                {
                    queue.bookList.RemoveAt(0);
                    _bookQueues.Update(queue);
                    _context.SaveChanges();
                }
            }
            else
            {
                Console.WriteLine("Queue not found.");
            }
        }

       


        public async Task UpdateQueue(string queueId, BookModel updatedBook)
        {
            var queue = await _bookQueues.FindAsync(queueId);
            if (queue != null)
            {
                var bookIndex = queue.bookList.FindIndex(b => b.BookId == updatedBook.BookId);
                if (bookIndex != -1)
                {
                    queue.bookList[bookIndex] = updatedBook;
                    _bookQueues.Update(queue);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine("Book not found in the queue.");
                }
            }
            else
            {
                Console.WriteLine("Queue not found.");
            }
        }
        public async Task ClearQueue(string id)
        {
            var queue = await _bookQueues.FindAsync(id);
            if (queue != null)
            {
                queue.bookList.Clear();
                _bookQueues.Update(queue);
                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Queue not found.");
            }
        }

        Task<bool> IQueueRepository.IsEmpty(string id)
        {
            BookQueue? queue =  _bookQueues.Find(id);
            if (queue != null)
            {
                if (queue.bookList.Count == 0)
                {
                      Console.WriteLine("The queue is empty.");
                      return Task.FromResult(true);
                }
                else
                {
                      Console.WriteLine("The queue is not empty.");
                        return Task.FromResult(false);
                }
                }
                else
                {
                 Console.WriteLine("Queue not found.");
                 return Task.FromResult(false);
            }
        }

        public async Task<BookQueue?> GetQueue(string queueId)
        {
            return await _bookQueues.FindAsync(queueId);
        }

        public async Task<bool> Contains(string queueId, string bookId)
        {
            var queue = await _bookQueues
                .Include(q => q.bookList)
                .FirstOrDefaultAsync(q => q.QueueId == queueId);

            if (queue == null)
                return false;

            return queue.bookList.Any(b => b.BookId == bookId);
        }
        }

    }