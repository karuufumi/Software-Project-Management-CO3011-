using backend.models;
using backend.repository;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository<UserModel> _userRepository;
        private readonly IMembershipRepository _membershipRepository;

        public UserController(IUserRepository<UserModel> userRepository, IMembershipRepository membershipRepository)
        {
            _userRepository = userRepository;
            _membershipRepository = membershipRepository;
        }

        // POST: api/user
        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserModel>>> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid model state",
                        Message = "Please provide valid user information"
                    });
                }

                UserModel user = request.Role switch
                {
                    "Student" => new Student
                    {
                        Username = request.Username,
                        Email = request.Email,
                        Role = "Student",
                        StudentId = request.RoleSpecificId ?? string.Empty,
                        Major = request.Major ?? string.Empty,
                        Year = request.Year ?? 1,
                        MembershipPoints = 0
                    },
                    "Faculty" => new FacultyMember
                    {
                        Username = request.Username,
                        Email = request.Email,
                        Role = "Faculty",
                        FacultyId = request.RoleSpecificId ?? string.Empty,
                        Department = request.Department ?? string.Empty,
                        Position = request.Position ?? string.Empty,
                        MembershipPoints = 0
                    },
                    "Librarian" => new Librarian
                    {
                        Username = request.Username,
                        Email = request.Email,
                        Role = "Librarian",
                        EmployeeId = request.RoleSpecificId ?? string.Empty,
                        HireDate = DateTime.UtcNow
                    },
                    "Admin" => new Admin
                    {
                        Username = request.Username,
                        Email = request.Email,
                        Role = "Admin",
                        AdminId = request.RoleSpecificId ?? string.Empty,
                        AccessLevel = request.AccessLevel ?? "SystemAdmin",
                        LastLogin = DateTime.UtcNow
                    },
                    _ => throw new ArgumentException("Invalid role specified")
                };

                await _userRepository.AddUser(user);

                // Create membership for Student/Faculty
                if (user is Student || user is FacultyMember)
                {
                    var membership = new Membership
                    {
                        UserId = user.Id,
                        Points = 0,
                        ExpiryDate = DateTime.UtcNow.AddYears(1) // Default 1 year
                    };
                    await _membershipRepository.AddMembership(membership);
                }

                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new ApiResponse<UserModel>
                {
                    Success = true,
                    Data = user,
                    Message = "User created successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to create user"
                });
            }
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserModel>>>> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllUsers();
                return Ok(new ApiResponse<IEnumerable<UserModel>>
                {
                    Success = true,
                    Data = users,
                    Message = "Users retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve users"
                });
            }
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserModel>>> GetUserById(string id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "User not found",
                        Message = $"User with ID {id} does not exist"
                    });
                }

                return Ok(new ApiResponse<UserModel>
                {
                    Success = true,
                    Data = user,
                    Message = "User retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve user"
                });
            }
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<UserModel>>> UpdateUser(string id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "User not found",
                        Message = $"User with ID {id} does not exist"
                    });
                }

                // Update common fields
                if (!string.IsNullOrEmpty(request.Username))
                    user.Username = request.Username;
                if (!string.IsNullOrEmpty(request.Email))
                    user.Email = request.Email;
                

                // Update role-specific fields
                if (user is Student student)
                {
                    if (!string.IsNullOrEmpty(request.Major))
                        student.Major = request.Major;
                    if (request.Year.HasValue)
                        student.Year = request.Year.Value;
                }
                else if (user is FacultyMember faculty)
                {
                    if (!string.IsNullOrEmpty(request.Department))
                        faculty.Department = request.Department;
                    if (!string.IsNullOrEmpty(request.Position))
                        faculty.Position = request.Position;
                }

                await _userRepository.UpdateUser(user);

                return Ok(new ApiResponse<UserModel>
                {
                    Success = true,
                    Data = user,
                    Message = "User updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to update user"
                });
            }
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteUser(string id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "User not found",
                        Message = $"User with ID {id} does not exist"
                    });
                }

                await _userRepository.RemoveUser(id);

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = id,
                    Message = "User deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to delete user"
                });
            }
        }

        // GET: api/user/{id}/borrows
        [HttpGet("{id}/borrows")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BorrowRecord>>>> GetUserBorrowHistory(string id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "User not found",
                        Message = $"User with ID {id} does not exist"
                    });
                }

                var borrowHistory = user.BorrowedBooks ?? Enumerable.Empty<BorrowRecord>();

                return Ok(new ApiResponse<IEnumerable<BorrowRecord>>
                {
                    Success = true,
                    Data = borrowHistory,
                    Message = "Borrow history retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve borrow history"
                });
            }
        }

        // GET: api/user/{id}/active-borrows
        [HttpGet("{id}/active-borrows")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BorrowRecord>>>> GetUserActiveBorrows(string id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "User not found",
                        Message = $"User with ID {id} does not exist"
                    });
                }

                var activeBorrows = user.BorrowedBooks?
                    .Where(b => !b.IsReturned && b.DueDate != null)
                    ?? Enumerable.Empty<BorrowRecord>();

                return Ok(new ApiResponse<IEnumerable<BorrowRecord>>
                {
                    Success = true,
                    Data = activeBorrows,
                    Message = "Active borrows retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve active borrows"
                });
            }
        }

        // GET: api/user/search?query=
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserModel>>>> SearchUsers([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid query",
                        Message = "Search query cannot be empty"
                    });
                }

                var allUsers = await _userRepository.GetAllUsers();
                var searchResults = allUsers.Where(u =>
                    u.Username.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    u.Id.Contains(query, StringComparison.OrdinalIgnoreCase)
                );

                return Ok(new ApiResponse<IEnumerable<UserModel>>
                {
                    Success = true,
                    Data = searchResults,
                    Message = $"Found {searchResults.Count()} user(s)"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to search users"
                });
            }
        }

        // PATCH: api/user/{id}/credit
        [HttpPatch("{id}/credit")]
        public async Task<ActionResult<ApiResponse<UserModel>>> UpdateUserCredit(string id, [FromBody] CreditUpdateRequest request)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "User not found",
                        Message = $"User with ID {id} does not exist"
                    });
                }

                if (user is not Student && user is not FacultyMember)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid user type",
                        Message = "Only Students and Faculty members have credit scores"
                    });
                }

                var updatedUser = await _userRepository.UpdateCredit(id, request.Points);

                return Ok(new ApiResponse<UserModel>
                {
                    Success = true,
                    Data = updatedUser,
                    Message = "Credit score updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to update credit score"
                });
            }
        }

        // GET: api/user/{id}/membership
        [HttpGet("{id}/membership")]
        public async Task<ActionResult<ApiResponse<int>>> GetUserMembershipPoints(string id)
        {
            try
            {
                var points = await _userRepository.GetUserMembership(id);
                
                return Ok(new ApiResponse<int>
                {
                    Success = true,
                    Data = points,
                    Message = "Membership points retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve membership points"
                });
            }
        }
    }

    // Request models
    public class CreateUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // "Student", "Faculty", "Librarian", "Admin"
        
        // Role-specific fields
        public string? RoleSpecificId { get; set; } // StudentId, FacultyId, EmployeeId, AdminId
        public string? Major { get; set; } // Student
        public int? Year { get; set; } // Student
        public string? Department { get; set; } // Faculty/Librarian
        public string? Position { get; set; } // Faculty
        public string? AccessLevel { get; set; } // Admin
    }

    public class UpdateUserRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Major { get; set; } // Student
        public int? Year { get; set; } // Student
        public string? Department { get; set; } // Faculty
        public string? Position { get; set; } // Faculty
    }

    public class CreditUpdateRequest
    {
        public int Points { get; set; }
    }
}