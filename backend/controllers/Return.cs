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
        
        private readonly IUserRepository<UserModel> _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IQueueRepository _queueRepository;

        public ReturnController(IUserRepository<UserModel> userRepository, IBookRepository bookRepository, IQueueRepository queueRepository)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _queueRepository = queueRepository;
        }

        public async Task ReturnBook(string userId, string bookId)
        {
            UserModel? user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            if (user.BorrowedBook != bookId)
            {
                throw new InvalidOperationException("This book was not borrowed by the user.");
            }

            BookModel? book = await _bookRepository.GetBookById(bookId);
            if (book == null)
            {
                throw new InvalidOperationException("Book not found.");
            }


            user.BorrowedBook = null;
            user.MembershipPoints += 10;

            await _userRepository.UpdateUser(user);

            book.Status = 0; // Assuming 0 means available
            await _bookRepository.UpdateBook(book);
        }
        
    } 
}