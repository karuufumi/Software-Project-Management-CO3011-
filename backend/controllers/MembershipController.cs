using backend.models;
using backend.repository;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUserRepository<UserModel> _userRepository;

        public MembershipController(
            IMembershipRepository membershipRepository,
            IUserRepository<UserModel> userRepository)
        {
            _membershipRepository = membershipRepository;
            _userRepository = userRepository;
        }

        // GET: api/membership/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<ApiResponse<Membership>>> GetMembership(string userId)
        {
            try
            {
                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "User not found",
                        Message = $"User with ID {userId} does not exist"
                    });
                }

                var membership = await _membershipRepository.GetMembershipByUserId(userId);
                if (membership == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "Membership not found",
                        Message = $"No membership found for user {userId}"
                    });
                }

                return Ok(new ApiResponse<Membership>
                {
                    Success = true,
                    Data = membership,
                    Message = "Membership retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve membership"
                });
            }
        }

        // POST: api/membership
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Membership>>> CreateMembership([FromBody] CreateMembershipDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid model state",
                        Message = "Please provide valid membership information"
                    });
                }

                var user = await _userRepository.GetUserById(request.UserId);
                if (user == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "User not found",
                        Message = $"User with ID {request.UserId} does not exist"
                    });
                }

                // Check if user already has a membership
                var existingMembership = await _membershipRepository.GetMembershipByUserId(request.UserId);
                if (existingMembership != null)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Membership already exists",
                        Message = $"User {request.UserId} already has a membership"
                    });
                }

                var membership = new Membership
                {
                    UserId = request.UserId,
                    Points = request.InitialPoints ?? 0,
                    ExpiryDate = request.ExpiryDate ?? DateTime.UtcNow.AddYears(1)
                };

                await _membershipRepository.AddMembership(membership);

                return CreatedAtAction(nameof(GetMembership), new { userId = request.UserId }, new ApiResponse<Membership>
                {
                    Success = true,
                    Data = membership,
                    Message = "Membership created successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to create membership"
                });
            }
        }

        // PUT: api/membership/{userId}/extend
        [HttpPut("{userId}/extend")]
        public async Task<ActionResult<ApiResponse<Membership>>> ExtendMembership(string userId, [FromBody] ExtendMembershipDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid model state",
                        Message = "Please provide valid extension information"
                    });
                }

                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "User not found",
                        Message = $"User with ID {userId} does not exist"
                    });
                }

                // Extend by months or set new expiry date
                if (request.Months.HasValue)
                {
                    await _membershipRepository.ExtendMembership(userId, request.Months.Value);
                }
                else if (request.NewExpiryDate.HasValue)
                {
                    await _membershipRepository.ExtendMembership(userId, request.NewExpiryDate.Value);
                }
                else
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid request",
                        Message = "Please provide either months or newExpiryDate"
                    });
                }

                var updatedMembership = await _membershipRepository.GetMembershipByUserId(userId);

                return Ok(new ApiResponse<Membership>
                {
                    Success = true,
                    Data = updatedMembership,
                    Message = "Membership extended successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiError
                {
                    Error = ex.Message,
                    Message = "Cannot extend membership"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to extend membership"
                });
            }
        }

        // PATCH: api/membership/{userId}/points
        [HttpPatch("{userId}/points")]
        public async Task<ActionResult<ApiResponse<Membership>>> UpdatePoints(string userId, [FromBody] UpdatePointsDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid model state",
                        Message = "Please provide valid points information"
                    });
                }

                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "User not found",
                        Message = $"User with ID {userId} does not exist"
                    });
                }

                // Add or deduct points based on positive/negative value
                if (request.Points > 0)
                {
                    await _membershipRepository.AddPoints(userId, request.Points);
                }
                else if (request.Points < 0)
                {
                    await _membershipRepository.DeductPoints(userId, Math.Abs(request.Points));
                }
                else
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid points value",
                        Message = "Points value cannot be zero"
                    });
                }

                var updatedMembership = await _membershipRepository.GetMembershipByUserId(userId);

                return Ok(new ApiResponse<Membership>
                {
                    Success = true,
                    Data = updatedMembership,
                    Message = request.Points > 0 
                        ? $"Added {request.Points} points successfully" 
                        : $"Deducted {Math.Abs(request.Points)} points successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiError
                {
                    Error = ex.Message,
                    Message = "Cannot update points"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to update points"
                });
            }
        }

        // GET: api/membership/{userId}/tier
        [HttpGet("{userId}/tier")]
        public async Task<ActionResult<ApiResponse<MembershipTierInfo>>> GetMembershipTier(string userId)
        {
            try
            {
                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "User not found",
                        Message = $"User with ID {userId} does not exist"
                    });
                }

                var membership = await _membershipRepository.GetMembershipByUserId(userId);
                if (membership == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "Membership not found",
                        Message = $"No membership found for user {userId}"
                    });
                }

                var tierInfo = GetTierInfo(membership.Points);

                return Ok(new ApiResponse<MembershipTierInfo>
                {
                    Success = true,
                    Data = tierInfo,
                    Message = "Membership tier retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve membership tier"
                });
            }
        }

        // GET: api/membership/expired
        [HttpGet("expired")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Membership>>>> GetExpiredMemberships()
        {
            try
            {
                // Get all users and check their memberships
                var allUsers = await _userRepository.GetAllUsers();
                var expiredMemberships = new List<Membership>();

                foreach (var user in allUsers)
                {
                    var membership = await _membershipRepository.GetMembershipByUserId(user.Id);
                    if (membership != null && membership.ExpiryDate < DateTime.UtcNow)
                    {
                        expiredMemberships.Add(membership);
                    }
                }

                return Ok(new ApiResponse<IEnumerable<Membership>>
                {
                    Success = true,
                    Data = expiredMemberships,
                    Message = $"Found {expiredMemberships.Count} expired membership(s)"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve expired memberships"
                });
            }
        }

        // DELETE: api/membership/{userId}
        [HttpDelete("{userId}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteMembership(string userId)
        {
            try
            {
                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "User not found",
                        Message = $"User with ID {userId} does not exist"
                    });
                }

                var membership = await _membershipRepository.GetMembershipByUserId(userId);
                if (membership == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "Membership not found",
                        Message = $"No membership found for user {userId}"
                    });
                }

                await _membershipRepository.RemoveMembership(userId);

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = userId,
                    Message = "Membership deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to delete membership"
                });
            }
        }

        // GET: api/membership/{userId}/status
        [HttpGet("{userId}/status")]
        public async Task<ActionResult<ApiResponse<MembershipStatus>>> GetMembershipStatus(string userId)
        {
            try
            {
                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "User not found",
                        Message = $"User with ID {userId} does not exist"
                    });
                }

                var membership = await _membershipRepository.GetMembershipByUserId(userId);
                if (membership == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "Membership not found",
                        Message = $"No membership found for user {userId}"
                    });
                }

                var isActive = await _membershipRepository.IsMembershipActive(userId);
                var daysUntilExpiry = (membership.ExpiryDate - DateTime.UtcNow).Days;
                var tierInfo = GetTierInfo(membership.Points);

                var status = new MembershipStatus
                {
                    IsActive = isActive,
                    Points = membership.Points,
                    ExpiryDate = membership.ExpiryDate,
                    DaysUntilExpiry = daysUntilExpiry > 0 ? daysUntilExpiry : 0,
                    Tier = tierInfo.TierName,
                    MaxBorrowLimit = tierInfo.MaxBorrowLimit
                };

                return Ok(new ApiResponse<MembershipStatus>
                {
                    Success = true,
                    Data = status,
                    Message = "Membership status retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve membership status"
                });
            }
        }

        // Helper method to determine membership tier
        private MembershipTierInfo GetTierInfo(int points)
        {
            if (points >= 1000)
            {
                return new MembershipTierInfo
                {
                    TierName = "Platinum",
                    MaxBorrowLimit = 10,
                    MinPoints = 1000,
                    MaxPoints = int.MaxValue,
                    CurrentPoints = points
                };
            }
            else if (points >= 500)
            {
                return new MembershipTierInfo
                {
                    TierName = "Gold",
                    MaxBorrowLimit = 7,
                    MinPoints = 500,
                    MaxPoints = 999,
                    CurrentPoints = points
                };
            }
            else if (points >= 100)
            {
                return new MembershipTierInfo
                {
                    TierName = "Silver",
                    MaxBorrowLimit = 5,
                    MinPoints = 100,
                    MaxPoints = 499,
                    CurrentPoints = points
                };
            }
            else
            {
                return new MembershipTierInfo
                {
                    TierName = "Bronze",
                    MaxBorrowLimit = 3,
                    MinPoints = 0,
                    MaxPoints = 99,
                    CurrentPoints = points
                };
            }
        }
    }

    // DTOs
    public class CreateMembershipDto
    {
        public string UserId { get; set; } = string.Empty;
        public int? InitialPoints { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class ExtendMembershipDto
    {
        public int? Months { get; set; }
        public DateTime? NewExpiryDate { get; set; }
    }

    public class UpdatePointsDto
    {
        public int Points { get; set; } // Positive = add, Negative = deduct
    }

    public class MembershipTierInfo
    {
        public string TierName { get; set; } = string.Empty;
        public int MaxBorrowLimit { get; set; }
        public int MinPoints { get; set; }
        public int MaxPoints { get; set; }
        public int CurrentPoints { get; set; }
    }

    public class MembershipStatus
    {
        public bool IsActive { get; set; }
        public int Points { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DaysUntilExpiry { get; set; }
        public string Tier { get; set; } = string.Empty;
        public int MaxBorrowLimit { get; set; }
    }
}