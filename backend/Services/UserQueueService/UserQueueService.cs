using AutoMapper;
using backend.Models.Domain;
using backend.Models.DTOs.Request;
using backend.Models.DTOS.Response;
using backend.Repositories;

namespace backend.Services.UserQueueService
{
    public class UserQueueService : IUserQueueService
    {
        private readonly IUserQueueRepository _userQueueRepository;
        private readonly IMapper _mapper;

        public UserQueueService(IUserQueueRepository userQueueRepository, IMapper mapper)
        {
            _userQueueRepository = userQueueRepository;
            _mapper = mapper;
        }

        public async Task<UserQueueResponseDTO> EnqueueUser(UserQueueRequestDTO userQueueRequest)
        {
            var queue = _mapper.Map<UserQueue>(userQueueRequest);

            var existingQueue = await _userQueueRepository.GetUserQueue(queue.BookId, queue.UserId);
            if (existingQueue != null)
            {
                return _mapper.Map<UserQueueResponseDTO>(existingQueue);
            }
            queue.CreatedAt = DateTime.UtcNow;

            await _userQueueRepository.CreateUserQueue(queue);
            return _mapper.Map<UserQueueResponseDTO>(queue);
        }

        public async Task<UserQueueResponseDTO?> DequeueUser(int bookId)
        {
            var queue = await _userQueueRepository.GetUserQueueByBookId(bookId);
            if (queue.Count() > 0)
            {
                var oldestQueue = queue.ElementAt(0);
                await _userQueueRepository.DeleteUserQueue(oldestQueue);
                return _mapper.Map<UserQueueResponseDTO>(oldestQueue);
            }
            return null;
        }

        public async Task<IEnumerable<UserQueueResponseDTO>> GetAllUserQueue()
        {
            var queues = await _userQueueRepository.GetAllUserQueue();
            return _mapper.Map<IEnumerable<UserQueueResponseDTO>>(queues);
        }

        public async Task<UserQueueResponseDTO?> GetUserQueue(int bookId, int userId)
        {
            var queue = await _userQueueRepository.GetUserQueue(bookId, userId);
            return queue == null ? null : _mapper.Map<UserQueueResponseDTO>(queue);
        }

        public async Task<IEnumerable<UserQueueResponseDTO>> GetUserQueueByBookId(int bookId)
        {
            var queues = await _userQueueRepository.GetUserQueueByBookId(bookId);
            return _mapper.Map<IEnumerable<UserQueueResponseDTO>>(queues);
        }
    }
}