
using backend.repository;
using Microsoft.AspNetCore.Mvc;

//for now, no realtime logic
namespace backend.controllers
{
    [ApiController]
    [Route("/")]
    public class BorrowController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BorrowController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpPost("{id}/borrow")]
        public async Task<IActionResult> BorrowBook(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            if (!book.IsAvailable)
            {
                return BadRequest("Book is already borrowed.");
            }

            await _bookRepository.UpdateStatus(id);
            return Ok("Book borrowed successfully.");
        }

        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            if (!book.IsAvailable)
            {
                return BadRequest("Book is not currently borrowed.");
            }

            await _bookRepository.UpdateStatus(id);
            return Ok("Book returned successfully.");
        }
    }
}