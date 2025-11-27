using backend.models;
using backend.repository;
using Microsoft.AspNetCore.Mvc;

//for now, no realtime logic
namespace backend.controllers
{
    [ApiController]
    [Route("api/borrow/[controller]")]

    public class BorrowController : ControllerBase
    {

        private readonly IUserRepository<UserModel> _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IQueueRepository _queueRepository;

        public BorrowController(IUserRepository<UserModel> userRepository, IBookRepository bookRepository, IQueueRepository queueRepository)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _queueRepository = queueRepository;
        }
        
        private MembershipLevel checkLevel(int membershipPoints)
        {
            if (membershipPoints >= 1000)
            {
                return MembershipLevel.VIP;
            }
            else if (membershipPoints >= 500)
            {
                return MembershipLevel.Premium;
            }
            else
            {
                return MembershipLevel.Standard;
            }
        }

        public Task BorrowBook(int userId,string bookId)
        {
            if (userId <= 0 || string.IsNullOrEmpty(bookId))
            {
                throw new ArgumentException("User ID must be a positive integer and Book ID must be a valid string.");
            }

            UserModel? user = _userRepository.GetUserById(userId).Result;
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");

            }
            // Logic to borrow the book goes here
            if (user.BorrowedBook != null)
            {
                PutIntoQueue(userId, bookId);
                return Task.CompletedTask;
            }
            user.BorrowedBook = bookId;
            return _userRepository.UpdateUser(user);


        }

        public Task PutIntoQueue(int userId, string bookId)
        {
            if (userId <= 0 || string.IsNullOrEmpty(bookId))
            {
                throw new ArgumentException("User ID must be a positive integer and Book ID must be a valid string.");
            }
            
        }

        // ...existing code...

        public async Task BorrowBook(string userId, string bookId)
        {
            
            UserModel? user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            // Logic to borrow the book goes here
            // This is a placeholder; actual implementation may vary
            // For example, check if the user can borrow the book, update records, etc.

            BookModel? book = await _bookRepository.GetBookById(bookId);
            if (book == null)
            {
                throw new InvalidOperationException("Book not found.");
            }

            int membershipPoints = await _userRepository.GetUserMembership(userId);

            MembershipLevel userLevel = checkLevel(membershipPoints);
            MembershipLevel bookLevel = book.GetMembershipLevel();
            if (userLevel < bookLevel)
            {
                throw new InvalidOperationException("User membership level is insufficient to borrow this book.");
            }
           
            if (book.Status != BookStatus.Available || user.BorrowedBook != null)
            {
                //implement borrow here
                
                Console.WriteLine($"User {userId} borrowed book {bookId}");
                user.BorrowedBook = bookId;
                book.Status = BookStatus.Borrowed;
                await _userRepository.UpdateUser(user);
                await _bookRepository.UpdateBook(book);
            }

            else
            {
                //throw new InvalidOperationException("Book is not available for borrowing or user has already borrowed a book.");
              //  BookQueue queue = await _queueRepository.GetQueue(userId);
              BookQueue ? queue = await _queueRepository.GetQueueById(userId); 
                if (queue == null)
                {
                    throw new InvalidOperationException("Queue not found.");
                }
                await _queueRepository.Push(book, queue.QueueId);
                Console.WriteLine($"Book {bookId} added to queue for user {userId}");
            }
        }
        }
        }
    