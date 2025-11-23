using backend.Helpers.Extensions;
using backend.Models.Domain;
using backend.Models.DTOs.Request;
using backend.Models.DTOS.Response;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("/api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDTO<IEnumerable<UserResponseDTO>>>> GetUsers()
        {
            var users = await _userService.GetAllUser();

            return this.ApiOk(users, "Get list users successfully");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponseDTO<UserResponseDTO>>> GetUserByID(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return this.ApiNotFound<User>("User not found");
            }
            return this.ApiOk(user, "Get user successfully");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequestDTO user)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return this.ApiBadRequest<object>("Validation failed", errors);
            }

            var res = await _userService.CreateUser(user);
            return this.ApiCreated(res, "User created successfully");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponseDTO<UserResponseDTO>>> UpdateUser(int id, UpdateUserRequestDTO updateUserRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return this.ApiBadRequest<object>("Validation failed", errors);
            }

            var user = await _userService.UpdateUser(id, updateUserRequest);
            if (user == null)
            {
                return this.ApiNotFound<User>("User not found");
            }
            return this.ApiOk(user, "Update user successfully");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.DeleteUser(id);
            if (user == null)
            {
                return this.ApiNotFound<User>("User not found");
            }
            return this.ApiOk<object>(null, "User deleted successfully");
        }
    }
}