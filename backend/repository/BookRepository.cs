using backend.Data;
using backend.models;


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
            _context.Set<BookModel>().Add(book);
            await  _context.SaveChangesAsync();
        }

        public async Task<BookModel?> GetBookById(int id)
        {
            return await _context.Set<BookModel>().FindAsync(id);
        }

        public async Task RemoveBook(int id)
        {
            var book = await _context.Set<BookModel>().FindAsync(id);
            if (book != null)
            {
                _context.Set<BookModel>().Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateBook(BookModel book)
        {
            _context.Set<BookModel>().Update(book);
            await _context.SaveChangesAsync();
        }

        public Task UpdateStatus(int id)
        {
            // update the availability status of the book

            throw new NotImplementedException();
        }
    }
}