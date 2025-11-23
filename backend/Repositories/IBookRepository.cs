using backend.Models.Domain;

namespace backend.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBook();

        Task<Book?> GetById(int id);

        Task CreateBook(Book bookModel);

        Task UpdateBook(Book bookDto);

        Task DeleteBook(Book bookDto);

    }
}