using backend.Data;
using backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.UserQueueRepository
{
    public class UserQueueRepository : IUserQueueRepository
    {
        private readonly AppDbContext _context;

        public UserQueueRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateUserQueue(UserQueue userQueue)
        {
            await _context.UserQueues.AddAsync(userQueue);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserQueue(UserQueue userQueue)
        {
            _context.UserQueues.Remove(userQueue);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserQueue>> GetAllUserQueue()
        {
            return await _context.UserQueues
                                .Include(u => u.User)
                                .Include(u => u.Book)
                                .ToListAsync();
        }

        public async Task<UserQueue?> GetUserQueue(int bookId, int userId)
        {
            return await _context.UserQueues
                            .Include(u => u.User)
                            .Include(u => u.Book)
                            .FirstOrDefaultAsync(q => q.BookId == bookId && q.UserId == userId);
        }

        public async Task<IEnumerable<UserQueue>> GetUserQueueByBookId(int bookId)
        {
            return await _context.UserQueues
                        .Where(u => u.BookId == bookId)
                        .Include(u => u.User)
                        .Include(u => u.Book)
                        .OrderBy(p => p.CreatedAt)
                        .ToListAsync();
        }
    }
}