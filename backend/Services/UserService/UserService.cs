using AutoMapper;
using backend.Models.Domain;
using backend.Models.DTOs.Request;
using backend.Models.DTOS.Response;
using backend.Repositories;

namespace backend.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserResponseDTO> CreateUser(CreateUserRequestDTO userRequestDTO)
        {
            var user = _mapper.Map<User>(userRequestDTO);
            await _userRepository.CreateUser(user);
            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO?> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByID(id);
            if (user != null)
            {
                await _userRepository.DeleteUser(user);
                return _mapper.Map<UserResponseDTO>(user);
            }
            return null;
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllUser()
        {
            var users = await _userRepository.GetAllUsers();
            return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
        }

        public async Task<UserResponseDTO?> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByID(id);
            return user == null ? null : _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO?> UpdateUser(int id, UpdateUserRequestDTO userRequestDTO)
        {
            var existingUser = await _userRepository.GetUserByID(id);
            if (existingUser != null)
            {
                _mapper.Map(userRequestDTO, existingUser);
                await _userRepository.UpdateUser(existingUser);
                return _mapper.Map<UserResponseDTO>(existingUser);
            }
            return null;
        }
    }
}