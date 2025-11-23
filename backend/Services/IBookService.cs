using backend.Models.DTOs.Request;
using backend.Models.DTOS.Response;

namespace backend.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookResponseDTO>> GetAllBook();

        Task<BookResponseDTO?> GetById(int id);

        Task<BookResponseDTO> CreateBook(CreateBookRequestDTO bookModel);

        Task<BookResponseDTO?> UpdateBook(int id, UpdateBookRequestDTO bookDto);

        Task<BookResponseDTO?> DeleteBook(int id);
    }
}