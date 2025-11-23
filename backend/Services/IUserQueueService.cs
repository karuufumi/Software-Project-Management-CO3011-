using backend.Models.DTOs.Request;
using backend.Models.DTOS.Response;

namespace backend.Services
{
    public interface IUserQueueService
    {
        Task<IEnumerable<UserQueueResponseDTO>> GetAllUserQueue();

        Task<IEnumerable<UserQueueResponseDTO>> GetUserQueueByBookId(int bookId);
        Task<UserQueueResponseDTO?> GetUserQueue(int bookId, int userId);

        Task<UserQueueResponseDTO> EnqueueUser(UserQueueRequestDTO userQueueRequest);

        Task<UserQueueResponseDTO?> DequeueUser(int bookId);
    }
}