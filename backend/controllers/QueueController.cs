using backend.models;
using backend.repository;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueueController : ControllerBase
    {
        private readonly IQueueRepository _queueRepository;
        private readonly IUserRepository<UserModel> _userRepository;
        private readonly IBookRepository _bookRepository;

        public QueueController(
            IQueueRepository queueRepository,
            IUserRepository<UserModel> userRepository,
            IBookRepository bookRepository)
        {
            _queueRepository = queueRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }

        // POST: api/queue/add
        [HttpPost("add")]
        public async Task<ActionResult<ApiResponse<string>>> AddBookToQueue([FromBody] QueueRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid model state",
                        Message = "Please provide valid queue information"
                    });
                }

                // Check if user exists
                var user = await _userRepository.GetUserById(request.UserId);
                if (user == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "User not found",
                        Message = $"User with ID {request.UserId} does not exist"
                    });
                }

                // Check if book exists
                var book = await _bookRepository.GetBookById(request.BookId);
                if (book == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "Book not found",
                        Message = $"Book with ID {request.BookId} does not exist"
                    });
                }

                // Check if queue exists, if not create one
                var queue = await _queueRepository.GetQueueById(request.UserId);
                if (queue == null)
                {
                    await _queueRepository.CreateQueue(request.UserId);
                    queue = await _queueRepository.GetQueueById(request.UserId);
                }

                await _queueRepository.Push(queue!.QueueId, request.BookId);

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = $"Book '{book.Title}' added to queue",
                    Message = "Book added to queue successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiError
                {
                    Error = ex.Message,
                    Message = "Cannot add book to queue"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to add book to queue"
                });
            }
        }

        // DELETE: api/queue/remove
        [HttpDelete("remove")]
        public async Task<ActionResult<ApiResponse<string>>> RemoveBookFromQueue([FromBody] QueueRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid model state",
                        Message = "Please provide valid queue information"
                    });
                }

                var queue = await _queueRepository.GetQueueById(request.UserId);
                if (queue == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "Queue not found",
                        Message = $"No queue found for user {request.UserId}"
                    });
                }

                await _queueRepository.Pop(queue.QueueId, request.BookId);

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = $"Book removed from queue",
                    Message = "Book removed from queue successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiError
                {
                    Error = ex.Message,
                    Message = "Cannot remove book from queue"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to remove book from queue"
                });
            }
        }

        // GET: api/queue/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<ApiResponse<BookQueue>>> GetUserQueue(string userId)
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

                var queue = await _queueRepository.GetQueueById(userId);
                if (queue == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "Queue not found",
                        Message = $"No queue found for user {userId}"
                    });
                }

                return Ok(new ApiResponse<BookQueue>
                {
                    Success = true,
                    Data = queue,
                    Message = "Queue retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve queue"
                });
            }
        }

        // GET: api/queue/{userId}/books
        [HttpGet("{userId}/books")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BookModel>>>> GetQueueBooks(string userId)
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

                var queue = await _queueRepository.GetQueueById(userId);
                if (queue == null)
                {
                    return Ok(new ApiResponse<IEnumerable<BookModel>>
                    {
                        Success = true,
                        Data = Enumerable.Empty<BookModel>(),
                        Message = "No queue found. Queue is empty."
                    });
                }

                var books = await _queueRepository.GetQueueBooks(queue.QueueId);

                return Ok(new ApiResponse<IEnumerable<BookModel>>
                {
                    Success = true,
                    Data = books,
                    Message = $"Found {books.Count()} book(s) in queue"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve queue books"
                });
            }
        }

        // DELETE: api/queue/{userId}/clear
        [HttpDelete("{userId}/clear")]
        public async Task<ActionResult<ApiResponse<string>>> ClearQueue(string userId)
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

                var queue = await _queueRepository.GetQueueById(userId);
                if (queue == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "Queue not found",
                        Message = $"No queue found for user {userId}"
                    });
                }

                await _queueRepository.ClearQueue(queue.QueueId);

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = $"Queue cleared for user {userId}",
                    Message = "Queue cleared successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to clear queue"
                });
            }
        }

        // GET: api/queue/check?userId=&bookId=
        [HttpGet("check")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckBookInQueue([FromQuery] string userId, [FromQuery] string bookId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(bookId))
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid parameters",
                        Message = "Both userId and bookId are required"
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

                var queue = await _queueRepository.GetQueueById(userId);
                if (queue == null)
                {
                    return Ok(new ApiResponse<bool>
                    {
                        Success = true,
                        Data = false,
                        Message = "No queue found for user"
                    });
                }

                var isInQueue = queue.QueuedBooks?.Any(b => b.BookId == bookId) ?? false;

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = isInQueue,
                    Message = isInQueue ? "Book is in queue" : "Book is not in queue"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to check book in queue"
                });
            }
        }

        // POST: api/queue/create/{userId}
        [HttpPost("create/{userId}")]
        public async Task<ActionResult<ApiResponse<BookQueue>>> CreateQueue(string userId)
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

                var existingQueue = await _queueRepository.GetQueueById(userId);
                if (existingQueue != null)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Queue already exists",
                        Message = $"Queue already exists for user {userId}"
                    });
                }

                await _queueRepository.CreateQueue(userId);
                var newQueue = await _queueRepository.GetQueueById(userId);

                return CreatedAtAction(nameof(GetUserQueue), new { userId }, new ApiResponse<BookQueue>
                {
                    Success = true,
                    Data = newQueue,
                    Message = "Queue created successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiError
                {
                    Error = ex.Message,
                    Message = "Cannot create queue"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to create queue"
                });
            }
        }

        // GET: api/queue/{userId}/isEmpty
        [HttpGet("{userId}/isEmpty")]
        public async Task<ActionResult<ApiResponse<bool>>> IsQueueEmpty(string userId)
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

                var queue = await _queueRepository.GetQueueById(userId);
                if (queue == null)
                {
                    return Ok(new ApiResponse<bool>
                    {
                        Success = true,
                        Data = true,
                        Message = "No queue found (considered empty)"
                    });
                }

                var isEmpty = await _queueRepository.IsEmpty(queue.QueueId);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = isEmpty,
                    Message = isEmpty ? "Queue is empty" : "Queue has items"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to check if queue is empty"
                });
            }
        }

        // GET: api/queue/{userId}/count
        [HttpGet("{userId}/count")]
        public async Task<ActionResult<ApiResponse<int>>> GetQueueCount(string userId)
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

                var queue = await _queueRepository.GetQueueById(userId);
                if (queue == null)
                {
                    return Ok(new ApiResponse<int>
                    {
                        Success = true,
                        Data = 0,
                        Message = "No queue found"
                    });
                }

                var count = queue.QueuedBooks?.Count ?? 0;

                return Ok(new ApiResponse<int>
                {
                    Success = true,
                    Data = count,
                    Message = $"Queue has {count} item(s)"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to get queue count"
                });
            }
        }
    }

    // DTOs
    public class QueueRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;
    }
}