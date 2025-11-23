using backend.Models.DTOs.Request;
using backend.Models.DTOS.Response;

namespace backend.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDTO>> GetAllUser();

        Task<UserResponseDTO?> GetUserById(int id);

        Task<UserResponseDTO> CreateUser(CreateUserRequestDTO userRequestDTO);

        Task<UserResponseDTO?> UpdateUser(int id, UpdateUserRequestDTO userRequestDTO);

        Task<UserResponseDTO?> DeleteUser(int id);
    }
}