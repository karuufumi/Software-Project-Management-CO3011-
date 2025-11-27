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

        private MembershipLevel CheckLevel(int points)
        {
            if (points >= 1000) return MembershipLevel.VIP;
            if (points >= 500)  return MembershipLevel.Premium;
            return MembershipLevel.Standard;
        }

        public async Task BorrowAsync(BorrowReqModel req)
        {
            // 1. Validate input
            if (string.IsNullOrEmpty(req.UserId) || string.IsNullOrEmpty(req.BookId))
                throw new ArgumentException("UserId and BookId cannot be empty.");

            // 2. Get user + book
            var user = await _userRepository.GetUserById(req.UserId);
            if (user == null) throw new InvalidOperationException("User not found.");

            var book = await _bookRepository.GetBookById(req.BookId);
            if (book == null) throw new InvalidOperationException("Book not found.");

            // 3. Membership check
            int membershipPoints = await _userRepository.GetUserMembership(req.UserId);
            var userLevel = CheckLevel(membershipPoints);
            var bookLevel = book.GetMembershipLevel();

            if (userLevel < bookLevel)
                throw new InvalidOperationException("User membership level is too low to borrow this book.");

            // 4. If user already has a borrowed book → add to queue
            if (user.BorrowedBook != null || book.Status != BookStatus.Available)
            {
                var queue = await _queueRepository.GetQueueById(req.UserId);
                if (queue == null)
                    throw new InvalidOperationException("Queue not found.");

                await _queueRepository.Push(book, queue.QueueId);
                return;
            }

            // 5. Borrow book
            user.BorrowedBook = req.BookId;

            book.Status = BookStatus.Borrowed;

            await _userRepository.UpdateUser(user);
            await _bookRepository.UpdateBook(book);
        }
        

           }
}
