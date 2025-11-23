using AutoMapper;
using backend.Data;
using backend.Models.Domain;
using backend.Models.DTOs.Request;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.BookRepository
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;
        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateBook(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBook(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBook()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetById(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task UpdateBook(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
    }
}