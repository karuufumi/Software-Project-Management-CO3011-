using backend.Data;
using backend.models;
using Microsoft.EntityFrameworkCore;

namespace backend.repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddBook(BookModel book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task<BookModel?> GetBookById(string id)
        {
            return await _context.Books
                .Include(b => b.BorrowRecords)
                .FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task<IEnumerable<BookModel>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task RemoveBook(string id)
        {
            var book = await GetBookById(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateBook(BookModel book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAvailability(string bookId, int availableCopies)
        {
            var book = await GetBookById(bookId);
            if (book != null)
            {
                book.AvailableCopies = availableCopies;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<BookModel>> SearchBooks(string searchTerm)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(searchTerm) || 
                           b.Author.Contains(searchTerm) || 
                           b.ISBN.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<BookModel>> GetBooksByGenre(string genre)
        {
            return await _context.Books
                .Where(b => b.Genre == genre)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookModel>> GetBooksByAuthor(string author)
        {
            return await _context.Books
                .Where(b => b.Author == author)
                .ToListAsync();
        }

        public Task<int> GetBookTier(string bookname)
        {
        
            var book =  _context.Books
                .FirstOrDefaultAsync(b => b.Title == bookname);
            if (book != null)
            {
                return Task.FromResult(book.Result.Tier);
            }
            throw new ArgumentException("Book not found");
        }
    }
}