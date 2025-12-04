using backend.models;
using backend.repository;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // POST: api/book
        [HttpPost]
        public async Task<ActionResult<ApiResponse<BookModel>>> AddBook([FromBody] BookModel book)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "Invalid model state",
                        Message = "Please provide valid book information"
                    });
                }

                await _bookRepository.AddBook(book);
                return CreatedAtAction(nameof(GetBookById), new { id = book.BookId }, new ApiResponse<BookModel>
                {
                    Success = true,
                    Data = book,
                    Message = "Book added successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to add book"
                });
            }
        }

        [HttpGet("tiers")]
        public async Task<ActionResult<ApiResponse<int>>> GetBookTiers(string bookname)
        {
            try
            {
                var tiers = await   _bookRepository.GetBookTier(bookname);
                return Ok(new ApiResponse<int>
                {
                    Success = true,
                    Data = tiers,
                    Message = "Book tiers retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve book tiers"
                });
            }
        }

        // GET: api/book
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<BookModel>>>> GetAllBooks()
        {
            try
            {
                var books = await _bookRepository.GetAllBooks();
                return Ok(new ApiResponse<IEnumerable<BookModel>>
                {
                    Success = true,
                    Data = books,
                    Message = "Books retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve books"
                });
            }
        }

        // GET: api/book/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BookModel>>> GetBookById(string id)
        {
            try
            {
                var book = await _bookRepository.GetBookById(id);
                if (book == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "Book not found",
                        Message = $"Book with ID {id} does not exist"
                    });
                }

                return Ok(new ApiResponse<BookModel>
                {
                    Success = true,
                    Data = book,
                    Message = "Book retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve book"
                });
            }
        }

        // PUT: api/book/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<BookModel>>> UpdateBook(string id, [FromBody] BookModel book)
        {
            try
            {
                if (id != book.BookId)
                {
                    return BadRequest(new ApiError
                    {
                        Error = "ID mismatch",
                        Message = "Book ID in URL does not match body"
                    });
                }

                var existingBook = await _bookRepository.GetBookById(id);
                if (existingBook == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "Book not found",
                        Message = $"Book with ID {id} does not exist"
                    });
                }

                await _bookRepository.UpdateBook(book);
                return Ok(new ApiResponse<BookModel>
                {
                    Success = true,
                    Data = book,
                    Message = "Book updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to update book"
                });
            }
        }

        // DELETE: api/book/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteBook(string id)
        {
            try
            {
                var book = await _bookRepository.GetBookById(id);
                if (book == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "Book not found",
                        Message = $"Book with ID {id} does not exist"
                    });
                }

                await _bookRepository.RemoveBook(id);
                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = id,
                    Message = "Book deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to delete book"
                });
            }
        }

        // GET: api/book/search?query=
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BookModel>>>> SearchBooks([FromQuery] string query)
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

                var books = await _bookRepository.SearchBooks(query);
                return Ok(new ApiResponse<IEnumerable<BookModel>>
                {
                    Success = true,
                    Data = books,
                    Message = $"Found {books.Count()} book(s)"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to search books"
                });
            }
        }

        // GET: api/book/genre/{genre}
        [HttpGet("genre/{genre}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BookModel>>>> GetBooksByGenre(string genre)
        {
            try
            {
                var books = await _bookRepository.GetBooksByGenre(genre);
                return Ok(new ApiResponse<IEnumerable<BookModel>>
                {
                    Success = true,
                    Data = books,
                    Message = $"Found {books.Count()} book(s) in {genre}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve books by genre"
                });
            }
        }

        // GET: api/book/author/{author}
        [HttpGet("author/{author}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BookModel>>>> GetBooksByAuthor(string author)
        {
            try
            {
                var books = await _bookRepository.GetBooksByAuthor(author);
                return Ok(new ApiResponse<IEnumerable<BookModel>>
                {
                    Success = true,
                    Data = books,
                    Message = $"Found {books.Count()} book(s) by {author}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve books by author"
                });
            }
        }

        // GET: api/book/available
        [HttpGet("available")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BookModel>>>> GetAvailableBooks()
        {
            try
            {
                var allBooks = await _bookRepository.GetAllBooks();
                var availableBooks = allBooks.Where(b => b.AvailableCopies > 0);
                
                return Ok(new ApiResponse<IEnumerable<BookModel>>
                {
                    Success = true,
                    Data = availableBooks,
                    Message = $"Found {availableBooks.Count()} available book(s)"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve available books"
                });
            }
        }

        // PATCH: api/book/{id}/availability
        [HttpPatch("{id}/availability")]
        public async Task<ActionResult<ApiResponse<BookModel>>> UpdateAvailability(string id, [FromBody] AvailabilityUpdateRequest request)
        {
            try
            {
                var book = await _bookRepository.GetBookById(id);
                if (book == null)
                {
                    return NotFound(new ApiError
                    {
                        Error = "Book not found",
                        Message = $"Book with ID {id} does not exist"
                    });
                }

                await _bookRepository.UpdateAvailability(id, request.AvailableCopies);
                book.AvailableCopies = request.AvailableCopies;

                return Ok(new ApiResponse<BookModel>
                {
                    Success = true,
                    Data = book,
                    Message = "Book availability updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to update book availability"
                });
            }
        }
    }

    // Response models
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ApiError
    {
        public bool Success { get; set; } = false;
        public string Error { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    // Request models
    public class AvailabilityUpdateRequest
    {
        public int AvailableCopies { get; set; }
    }
}