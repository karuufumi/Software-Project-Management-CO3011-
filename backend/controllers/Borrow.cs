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
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IQueueRepository _queueRepository;

        public BorrowController(IBookRepository bookRepository, IUserRepository userRepository, IQueueRepository queueRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _queueRepository = queueRepository;
        }

        // Implement borrowing logic here
        [HttpPost("{userId}/{bookId}")]
        public async Task<IActionResult> BorrowBook(int userId, int bookId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            if (user.BorrowedBook != null)
            {
                return BadRequest("User has already borrowed a book");
            }

            var book = await _bookRepository.GetBookById(bookId);
            if (book == null)
            {
                return NotFound("Book not found");
            }

            if (book.Status != BookStatus.Available)
            {
                // put the book into the queue for the user
                await _queueRepository.AddToQueue(book, userId);
                return Ok("Book is not available. Added to your borrow queue.");
            }

            // Update book status to Borrowed
            user.BorrowedBook = book;
            await _userRepository.UpdateUser(user);
            book.Status = BookStatus.Borrowed;
            await _bookRepository.UpdateBook(book);

            return Ok("Book borrowed successfully");
        }
    }
}