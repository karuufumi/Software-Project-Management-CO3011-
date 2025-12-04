using backend.models;

namespace backend.repository
{
    public interface IBookRepository
    {
        Task AddBook(BookModel book);
        Task<BookModel?> GetBookById(string id);
        Task<IEnumerable<BookModel>> GetAllBooks();
        Task RemoveBook(string id);
        Task UpdateBook(BookModel book);
        
        // Update book availability
        Task UpdateAvailability(string bookId, int availableCopies);
        
        // Search and filter methods
        Task<IEnumerable<BookModel>> SearchBooks(string searchTerm);
        Task<IEnumerable<BookModel>> GetBooksByGenre(string genre);
        Task<IEnumerable<BookModel>> GetBooksByAuthor(string author);
        Task<int> GetBookTier(string bookname);
    } 
}