using backend.models;
using backend.repository;
using Microsoft.AspNetCore.Mvc;


namespace backend.controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class LibrarianController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository<UserModel> _userRepository;
        private readonly IQueueRepository _queueRepository;

        public LibrarianController(IBookRepository bookRepository, IUserRepository<UserModel> userRepository, IQueueRepository queueRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _queueRepository = queueRepository;
        }


        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooks();
            return Ok(books);
        }

        // Define your actions here
    }
}