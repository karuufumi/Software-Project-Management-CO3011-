
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
        private readonly IUserRepository _userRepository;

    public const int PenaltyAmount = 100;

        public PenaltyController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Implement penalty logic here
        [HttpPost("{userId}")]
        public async Task<IActionResult> ApplyPenalty(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.MembershipPoints += PenaltyAmount;
            await _userRepository.UpdateUser(user);

            return Ok($"Applied {PenaltyAmount} penalty points to user {user.Name}");
        }
    }
    
}