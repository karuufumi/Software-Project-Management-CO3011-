using backend.Data;
using backend.models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class QueueRepository : IQueueRepository
    {
        private readonly ApplicationDbContext _context;

        public QueueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. CORE ACTIONS (CRUD)
        // ==========================================

        public async Task AddAsync(BorrowRequest item)
        {
            await _context.BorrowRequests.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BorrowRequest item)
        {
            _context.BorrowRequests.Update(item);
            await _context.SaveChangesAsync();
        }
        
        public async Task<BorrowRequest?> GetRequestByIdAsync(int id)
        {
            return await _context.BorrowRequests
                .Include(br => br.User) // Load User details
                .Include(br => br.Book) // Load Book details
                .FirstOrDefaultAsync(br => br.Id == id);
        }

        public async Task<List<BorrowRequest>> GetAllRequestsAsync()
        {
            return await _context.BorrowRequests
                .Include(br => br.Book)
                .Include(br => br.User)
                .ToListAsync();
        }

        // ==========================================
        // 2. QUEUE LOGIC (The "Brain")
        // ==========================================

        // Find the specific person who should get the book next.
        // It looks for the OLDEST request that is still WAITING.
        public async Task<BorrowRequest?> GetNextPersonWaitingAsync(int bookId)
        {
            return await _context.BorrowRequests
                .Where(br => br.BookId == bookId 
                             && br.Status == RequestStatus.Waiting) 
                .OrderBy(br => br.RequestedAt) // First Come, First Served
                .FirstOrDefaultAsync();
        }

        // Check if a user is trying to join a line they are already in.
        // Returns TRUE if they are Waiting OR if the book is already Allocated to them.
        public async Task<bool> IsUserAlreadyInQueueAsync(int userId, int bookId)
        {
            return await _context.BorrowRequests
                .AnyAsync(br => br.UserId == userId 
                                && br.BookId == bookId 
                                && (br.Status == RequestStatus.Waiting || br.Status == RequestStatus.Allocated));
        }

        // Calculate "You are #5 in line".
        // Optimized: Uses SQL COUNT instead of downloading the whole list.
        public async Task<int> GetQueuePositionAsync(int userId, int bookId)
        {
            // A. Find the user's current request ticket
            var userRequest = await _context.BorrowRequests
                .FirstOrDefaultAsync(br => br.UserId == userId 
                                           && br.BookId == bookId 
                                           && br.Status == RequestStatus.Waiting);

            // If they aren't in line, return -1
            if (userRequest == null) return -1;

            // B. Count how many people have an EARLIER requested time
            var countAhead = await _context.BorrowRequests
                .CountAsync(br => br.BookId == bookId 
                                  && br.Status == RequestStatus.Waiting 
                                  && br.RequestedAt < userRequest.RequestedAt);

            return countAhead + 1; // 0 people ahead means position #1
        }

        // ==========================================
        // 3. MAINTENANCE (Background Jobs)
        // ==========================================

        // Find people who were told "Come pick it up" but never showed up.
        public async Task<List<BorrowRequest>> GetExpiredAllocationsAsync()
        {
            return await _context.BorrowRequests
                .Where(br => br.Status == RequestStatus.Allocated 
                             && br.AllocationExpiry.HasValue 
                             && br.AllocationExpiry.Value < DateTime.UtcNow) // Always use UtcNow
                .ToListAsync();
        }
    }
}