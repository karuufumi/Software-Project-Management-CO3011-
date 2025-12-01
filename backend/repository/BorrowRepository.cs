using backend.models;

namespace backend.repository
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly IUserRepository<UserModel> _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IQueueRepository _queueRepository;

        public BorrowRepository(
            IUserRepository<UserModel> userRepository,
            IBookRepository bookRepository,
            IQueueRepository queueRepository)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _queueRepository = queueRepository;
        }

        // User creates a borrow request
        public async Task CreateBorrowRequest(string userId, string bookId)
        {
            var user = await _userRepository.GetUserById(userId);
            var book = await _bookRepository.GetBookById(bookId);

            if (user == null || book == null)
            {
                throw new ArgumentException("Invalid user ID or book ID.");
            }

            // Check if book is available
            if (book.AvailableCopies <= 0)
            {
                throw new InvalidOperationException("Book is unavailable. Consider adding to queue.");
            }

            // Check borrow limits
            int membershipPoints = await _userRepository.GetUserMembership(userId);
            int maxBorrowLimit = GetBorrowLimit(membershipPoints);
            int currentBorrowCount = user.BorrowedBooks?.Count(b => !b.IsReturned) ?? 0;

            if (currentBorrowCount >= maxBorrowLimit)
            {
                throw new InvalidOperationException($"Borrow limit reached: {maxBorrowLimit} books.");
            }

            // Check if user already has pending or active borrow for this book
            if (user.BorrowedBooks?.Any(b => b.BookId == bookId && (!b.IsReturned || b.DueDate == null)) == true)
            {
                throw new InvalidOperationException("User already has a pending or active borrow for this book.");
            }

            // Create pending borrow request (no due date yet)
            var borrowRequest = new BorrowRecord
            {
                UserId = userId,
                BookId = bookId,
                BorrowDate = DateTime.UtcNow,
                DueDate = null, // Will be set by librarian
                ReturnDate = null,
                IsReturned = false
            };

            if (user.BorrowedBooks == null)
            {
                user.BorrowedBooks = new List<BorrowRecord>();
            }
            user.BorrowedBooks.Add(borrowRequest);
            await _userRepository.UpdateUser(user);
        }

        // Librarian approves and completes the borrow
        public async Task ApproveBorrowRequest(string userId, string bookId, DateTime dueDate)
        {
            var user = await _userRepository.GetUserById(userId);
            var book = await _bookRepository.GetBookById(bookId);

            if (user == null || book == null)
            {
                throw new ArgumentException("Invalid user ID or book ID.");
            }

            // Find the pending borrow request (one without due date)
            var borrowRequest = user.BorrowedBooks?.FirstOrDefault(b => 
                b.BookId == bookId && !b.IsReturned && b.DueDate == null);
            
            if (borrowRequest == null)
            {
                throw new InvalidOperationException("No pending borrow request found for this book.");
            }

            // Approve the request by setting the due date
            borrowRequest.DueDate = dueDate;

            // Update book availability
            book.AvailableCopies--;
            await _bookRepository.UpdateBook(book);

            // Update user record
            await _userRepository.UpdateUser(user);

            // Remove from queue if exists
            var queue = await _queueRepository.GetQueueById(userId);
            if (queue != null)
            {
            await _queueRepository.Pop(queue.QueueId, bookId);
            }
        }

        // Librarian records the return
        public async Task ReturnBook(string userId, string bookId, DateTime returnDate)
        {
            var user = await _userRepository.GetUserById(userId);
            var book = await _bookRepository.GetBookById(bookId);

            if (user == null || book == null)
            {
                throw new ArgumentException("Invalid user ID or book ID.");
            }

            var borrowRecord = user.BorrowedBooks?.FirstOrDefault(b => 
                b.BookId == bookId && !b.IsReturned && b.DueDate != null);
            
            if (borrowRecord == null)
            {
                throw new InvalidOperationException("No active borrow record found.");
            }

            borrowRecord.IsReturned = true;
            borrowRecord.ReturnDate = returnDate;

            // Check for late return and handle penalties
            if (returnDate > borrowRecord.DueDate)
            {
                int daysLate = (returnDate - borrowRecord.DueDate.Value).Days;
                // Apply penalties: could deduct membership points
                int penalty = daysLate * 10; // 10 points per day late
                await _userRepository.UpdateCredit(userId, -penalty);
            }

            book.AvailableCopies++;
            await _bookRepository.UpdateBook(book);
            await _userRepository.UpdateUser(user);
        }

       

        public async Task<IEnumerable<BorrowRecord>> GetPendingRequests()
        {
            // Query all users and get their pending borrow requests
            var allUsers = await _userRepository.GetAllUsers();
            var pendingRequests = allUsers
                .SelectMany(u => u.BorrowedBooks ?? Enumerable.Empty<BorrowRecord>())
                .Where(b => !b.IsReturned && b.DueDate == null)
                .ToList();
            
            return await Task.FromResult(pendingRequests);
        }

// ...existing code...

        public async Task<IEnumerable<BorrowRecord>> GetUserActiveBorrows(string userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            return user.BorrowedBooks?.Where(b => !b.IsReturned && b.DueDate != null) 
                ?? Enumerable.Empty<BorrowRecord>();
        }

        public async Task<IEnumerable<BorrowRecord>> GetUserBorrowHistory(string userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            return user.BorrowedBooks ?? Enumerable.Empty<BorrowRecord>();
        }

        private int GetBorrowLimit(int membershipPoints)
        {
            if (membershipPoints >= 1000) return 10;
            if (membershipPoints >= 500) return 7;
            if (membershipPoints >= 100) return 5;
            return 3;
        }
    }
}