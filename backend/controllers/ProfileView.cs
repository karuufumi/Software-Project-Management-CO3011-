
using backend.models;
using backend.repository;
using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.dtos;
using Microsoft.CodeAnalysis;


namespace backend.controllers
{
    
    [ApiController]
    [Route("api/profile/[controller]")]
    public class ProfileViewController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public ProfileViewController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var userProfile = new UserProfile(user);
            return Ok(userProfile);
        }
    }
}