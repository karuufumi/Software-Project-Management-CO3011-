
using backend.models;
using backend.repository;
using Microsoft.AspNetCore.Mvc;
//for now, no realtime logic
namespace backend.controllers
{
    [ApiController]
    [Route("api/penalty/[controller]")]
    public class PenaltyController : ControllerBase
    {
        private readonly IUserRepository<UserModel> _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IQueueRepository _queueRepository;

        public PenaltyController(IUserRepository<UserModel> userRepository, IBookRepository bookRepository, IQueueRepository queueRepository)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _queueRepository = queueRepository;
        }

        

        // Implement penalty-related actions here
    }
}