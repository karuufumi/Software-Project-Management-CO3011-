using backend.Helpers.Extensions;
using backend.Models.Domain;
using backend.Models.DTOs.Request;
using backend.Models.DTOS.Response;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("/api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;
        public BookController(IBookService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDTO<IEnumerable<BookResponseDTO>>>> GetBooks()
        {
            var books = await _service.GetAllBook();

            return this.ApiOk(books, "Get list books successfully");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponseDTO<BookResponseDTO>>> GetBookByID(int id)
        {
            var book = await _service.GetById(id);
            if (book == null)
            {
                return this.ApiNotFound<Book>("Book not found");
            }
            return this.ApiOk(book, "Get book successfully");
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(CreateBookRequestDTO book)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return this.ApiBadRequest<object>("Validation failed", errors);
            }

            var res = await _service.CreateBook(book);
            return this.ApiCreated(res, "Book created successfully");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponseDTO<BookResponseDTO>>> UpdateBook(int id, UpdateBookRequestDTO updateBookRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return this.ApiBadRequest<object>("Validation failed", errors);
            }

            var book = await _service.UpdateBook(id, updateBookRequest);
            if (book == null)
            {
                return this.ApiNotFound<Book>("Book not found");
            }
            return this.ApiOk(book, "Update book successfully");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _service.DeleteBook(id);
            if (book == null)
            {
                return this.ApiNotFound<Book>("Book not found");
            }
            return this.ApiOk<object>(null, "Book deleted successfully");
        }

    }
}