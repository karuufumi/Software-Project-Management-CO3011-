using backend.repository;
using backend.models;
using Microsoft.AspNetCore.Mvc;

/*
We will implement a Wishlist-like queue system for users to request books that are currently unavailable
or they are currently already borrow one.
*/
namespace backend.controllers
{
    [ApiController]
    [Route("api/queue/[controller]")]
    public class QueuingController : ControllerBase
    {
        
        private readonly IQueueRepository _queueRepository;
        private readonly IUserRepository<UserModel> _userRepository;
        private readonly IBookRepository _bookRepository;

        public QueuingController(IQueueRepository queueRepository, IUserRepository<UserModel> userRepository, IBookRepository bookRepository)
        {
            _queueRepository = queueRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }

        // ...existing code...

        public async Task PushIntoQueue(string userId, string bookId)
        {
            BookQueue? bookqueue = await _queueRepository.GetQueueById(userId);
            if (bookqueue == null)
            {
                throw new InvalidOperationException("Queue not found.");
            }

            BookModel? selectedBook = await _bookRepository.GetBookById(bookId); 
            if (selectedBook == null)
            {
                throw new InvalidOperationException("Book not found.");
            }
            await _queueRepository.Push(selectedBook, bookqueue.QueueId);
        }

        
        
        public async Task PopFromQueue(string userId)
        {
            BookQueue? bookqueue = await _queueRepository.GetQueueById(userId);
            if (bookqueue == null)
            {
                throw new InvalidOperationException("Queue not found.");
            }
            await _queueRepository.Pop(bookqueue.QueueId);
        }

        public async Task<bool> IsQueueEmpty(string userId)
        {
            BookQueue? bookqueue = await _queueRepository.GetQueueById(userId);
            if (bookqueue == null)
            {
                throw new InvalidOperationException("Queue not found.");
            }
            return await _queueRepository.IsEmpty(bookqueue.QueueId);
        }

    }
}