using backend.models;
using backend.repository;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowRepository _borrowRepository;
        private readonly IUserRepository<UserModel> _userRepository;
        private readonly IBookRepository _bookRepository;

        public BorrowController(
            IBorrowRepository borrowRepository,
            IUserRepository<UserModel> userRepository,
            IBookRepository bookRepository)
        {
            _borrowRepository = borrowRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }

        // POST: api/borrow/request
        [HttpPost("request")]
        public async Task<ActionResult<ApiResponse<string>>> CreateBorrowRequest([FromBody] BorrowRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid model state",
                        Message = "Please provide valid request information"
                    });
                }

                await _borrowRepository.CreateBorrowRequest(request.UserId, request.BookId);

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = $"Borrow request created for user {request.UserId} and book {request.BookId}",
                    Message = "Borrow request created successfully. Awaiting librarian approval."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiError
                {
                    Error = ex.Message,
                    Message = "Cannot create borrow request"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to create borrow request"
                });
            }
        }

        // GET: api/borrow/pending
        [HttpGet("pending")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BorrowRecord>>>> GetPendingRequests()
        {
            try
            {
                var pendingRequests = await _borrowRepository.GetPendingRequests();

                return Ok(new ApiResponse<IEnumerable<BorrowRecord>>
                {
                    Success = true,
                    Data = pendingRequests,
                    Message = $"Found {pendingRequests.Count()} pending request(s)"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve pending requests"
                });
            }
        }

        // POST: api/borrow/approve
        [HttpPost("approve")]
        public async Task<ActionResult<ApiResponse<string>>> ApproveBorrowRequest([FromBody] ApproveBorrowDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid model state",
                        Message = "Please provide valid approval information"
                    });
                }

                await _borrowRepository.ApproveBorrowRequest(request.UserId, request.BookId, request.DueDate);

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = $"Borrow approved for user {request.UserId} and book {request.BookId}",
                    Message = $"Borrow request approved. Due date: {request.DueDate:yyyy-MM-dd}"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiError
                {
                    Error = ex.Message,
                    Message = "Cannot approve borrow request"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to approve borrow request"
                });
            }
        }

        // POST: api/borrow/return
        [HttpPost("return")]
        public async Task<ActionResult<ApiResponse<string>>> ReturnBook([FromBody] ReturnBookDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid model state",
                        Message = "Please provide valid return information"
                    });
                }

                await _borrowRepository.ReturnBook(request.UserId, request.BookId, request.ReturnDate);

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = $"Book returned by user {request.UserId}",
                    Message = "Book returned successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiError
                {
                    Error = ex.Message,
                    Message = "Cannot process return"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to process book return"
                });
            }
        }

        // GET: api/borrow/active
        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BorrowRecord>>>> GetAllActiveBorrows()
        {
            try
            {
                var allUsers = await _userRepository.GetAllUsers();
                var activeBorrows = allUsers
                    .SelectMany(u => u.BorrowedBooks ?? Enumerable.Empty<BorrowRecord>())
                    .Where(b => !b.IsReturned && b.DueDate != null)
                    .ToList();

                return Ok(new ApiResponse<IEnumerable<BorrowRecord>>
                {
                    Success = true,
                    Data = activeBorrows,
                    Message = $"Found {activeBorrows.Count} active borrow(s)"
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

        // GET: api/borrow/overdue
        [HttpGet("overdue")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BorrowRecord>>>> GetOverdueBorrows()
        {
            try
            {
                var allUsers = await _userRepository.GetAllUsers();
                var overdueBorrows = allUsers
                    .SelectMany(u => u.BorrowedBooks ?? Enumerable.Empty<BorrowRecord>())
                    .Where(b => !b.IsReturned && 
                               b.DueDate != null && 
                               b.DueDate < DateTime.UtcNow)
                    .ToList();

                return Ok(new ApiResponse<IEnumerable<BorrowRecord>>
                {
                    Success = true,
                    Data = overdueBorrows,
                    Message = $"Found {overdueBorrows.Count} overdue borrow(s)"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve overdue borrows"
                });
            }
        }

        // GET: api/borrow/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BorrowRecord>>>> GetUserBorrows(string userId)
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

                var borrows = user.BorrowedBooks ?? Enumerable.Empty<BorrowRecord>();

                return Ok(new ApiResponse<IEnumerable<BorrowRecord>>
                {
                    Success = true,
                    Data = borrows,
                    Message = $"Found {borrows.Count()} borrow(s) for user"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve user borrows"
                });
            }
        }

        // GET: api/borrow/user/{userId}/active
        [HttpGet("user/{userId}/active")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BorrowRecord>>>> GetUserActiveBorrows(string userId)
        {
            try
            {
                var activeBorrows = await _borrowRepository.GetUserActiveBorrows(userId);

                return Ok(new ApiResponse<IEnumerable<BorrowRecord>>
                {
                    Success = true,
                    Data = activeBorrows,
                    Message = $"Found {activeBorrows.Count()} active borrow(s)"
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ApiError
                {
                    Error = ex.Message,
                    Message = "User not found"
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

        // GET: api/borrow/user/{userId}/history
        [HttpGet("user/{userId}/history")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BorrowRecord>>>> GetUserBorrowHistory(string userId)
        {
            try
            {
                var history = await _borrowRepository.GetUserBorrowHistory(userId);

                return Ok(new ApiResponse<IEnumerable<BorrowRecord>>
                {
                    Success = true,
                    Data = history,
                    Message = $"Retrieved borrow history for user"
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ApiError
                {
                    Error = ex.Message,
                    Message = "User not found"
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

        // DELETE: api/borrow/cancel/{borrowId}
        [HttpDelete("cancel/{borrowId}")]
        public async Task<ActionResult<ApiResponse<string>>> CancelBorrowRequest(string borrowId)
        {
            try
            {
                // Find the borrow record across all users
                var allUsers = await _userRepository.GetAllUsers();
                BorrowRecord? borrowToCancel = null;
                UserModel? userWithBorrow = null;

                foreach (var user in allUsers)
                {
                    borrowToCancel = user.BorrowedBooks?.FirstOrDefault(b => b.BorrowId == borrowId);
                    if (borrowToCancel != null)
                    {
                        userWithBorrow = user;
                        break;
                    }
                }

                if (borrowToCancel == null || userWithBorrow == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "Borrow record not found",
                        Message = $"Borrow with ID {borrowId} does not exist"
                    });
                }

                // Only allow canceling pending requests (no due date set)
                if (borrowToCancel.DueDate != null)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Cannot cancel approved borrow",
                        Message = "This borrow has already been approved and cannot be canceled"
                    });
                }

                userWithBorrow.BorrowedBooks?.Remove(borrowToCancel);
                await _userRepository.UpdateUser(userWithBorrow);

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = borrowId,
                    Message = "Borrow request canceled successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to cancel borrow request"
                });
            }
        }

        // GET: api/borrow/book/{bookId}
        [HttpGet("book/{bookId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BorrowRecord>>>> GetBorrowsByBook(string bookId)
        {
            try
            {
                var book = await _bookRepository.GetBookById(bookId);
                if (book == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "Book not found",
                        Message = $"Book with ID {bookId} does not exist"
                    });
                }

                var allUsers = await _userRepository.GetAllUsers();
                var borrowsForBook = allUsers
                    .SelectMany(u => u.BorrowedBooks ?? Enumerable.Empty<BorrowRecord>())
                    .Where(b => b.BookId == bookId)
                    .ToList();

                return Ok(new ApiResponse<IEnumerable<BorrowRecord>>
                {
                    Success = true,
                    Data = borrowsForBook,
                    Message = $"Found {borrowsForBook.Count} borrow record(s) for this book"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve borrow records for book"
                });
            }
        }
    }

    // DTOs (Data Transfer Objects)
    public class BorrowRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;
    }

    public class ApproveBorrowDto
    {
        public string UserId { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
    }

    public class ReturnBookDto
    {
        public string UserId { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;
        public DateTime ReturnDate { get; set; }
    }
}