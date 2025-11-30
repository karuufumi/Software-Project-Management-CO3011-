using backend.models;

namespace backend.repository
{
    public interface IBorrowRepository
    {
        // User creates a borrow request
        Task CreateBorrowRequest(string userId, string bookId);
        
        // Librarian approves and completes the borrow with due date
        Task ApproveBorrowRequest(string userId, string bookId, DateTime dueDate);
        
        // Librarian records the actual return with return date
        Task ReturnBook(string userId, string bookId, DateTime returnDate);
        
        // Get all pending borrow requests (optional, for librarian to view)
        Task<IEnumerable<BorrowRecord>> GetPendingRequests();
        
        // Get user's active borrows
        Task<IEnumerable<BorrowRecord>> GetUserActiveBorrows(string userId);
        
        // Get user's borrow history
        Task<IEnumerable<BorrowRecord>> GetUserBorrowHistory(string userId);
    }
}