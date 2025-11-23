using AutoMapper;
using backend.Data;
using backend.Models.Domain;
using backend.Models.DTOs.Request;
using backend.Models.DTOS.Response;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
        }

        public async Task<BookResponseDTO> CreateBook(CreateBookRequestDTO bookModel)
        {
            var book = _mapper.Map<Book>(bookModel);
            await _bookRepository.CreateBook(book);
            return _mapper.Map<BookResponseDTO>(book);
        }

        public async Task<BookResponseDTO?> DeleteBook(int id)
        {
            var bookModel = await _bookRepository.GetById(id);
            if (bookModel != null)
            {
                await _bookRepository.DeleteBook(bookModel);
                return _mapper.Map<BookResponseDTO>(bookModel);
            }
            return null;
        }

        public async Task<IEnumerable<BookResponseDTO>> GetAllBook()
        {
            var books = await _bookRepository.GetAllBook();
            return _mapper.Map<IEnumerable<BookResponseDTO>>(books);
        }

        public async Task<BookResponseDTO?> GetById(int id)
        {
            var book = await _bookRepository.GetById(id);
            return book == null ? null : _mapper.Map<BookResponseDTO>(book);
        }

        public async Task<BookResponseDTO?> UpdateBook(int id, UpdateBookRequestDTO bookDto)
        {
            var existingBook = await _bookRepository.GetById(id);
            if (existingBook != null)
            {
                _mapper.Map(bookDto, existingBook);
                await _bookRepository.UpdateBook(existingBook);
                return _mapper.Map<BookResponseDTO>(existingBook);
            }
            return null;
        }
    }
}