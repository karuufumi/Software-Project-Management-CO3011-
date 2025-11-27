using backend.Data;
using backend.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;


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
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task<BookModel?> GetBookById(string id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task RemoveBook(string id)
        {
            var book = await _context.Books.FindAsync(id);
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

        public async Task UpdateStatus(string id, int status)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                switch(status)
                {
                    case 0:
                        book.Status = BookStatus.Available;
                        break;
                    case 1:
                        book.Status = BookStatus.Borrowed;
                        break;
                 
                    default:
                        book.Status = BookStatus.Lost;
                        break;
                }
                
            }
            else
            {
                throw new Exception("Book not found");
            }
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
            }

        }
}