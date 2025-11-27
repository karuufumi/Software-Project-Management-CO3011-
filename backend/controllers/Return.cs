using backend.models;
using backend.repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;


namespace backend.controllers
{
    [ApiController]
    [Route("api/return/[controller]")]
    public class ReturnController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IQueueRepository _queueRepository;

        public ReturnController(IBookRepository bookRepository, IUserRepository userRepository, IQueueRepository queueRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _queueRepository = queueRepository;
        }

        // Implement return logic here
        [HttpPost("{userId}")]
        public async Task<IActionResult> ReturnBook(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var book = user.BorrowedBook;
            if (book == null)
            {
                return BadRequest("User has no borrowed book to return");
            }
            // Update book status to Available
            book.Status = BookStatus.Available;
            await _bookRepository.UpdateBook(book);
            user.BorrowedBook = null;
            await _userRepository.UpdateUser(user);

            return Ok("Book returned successfully");
        }
    }
}