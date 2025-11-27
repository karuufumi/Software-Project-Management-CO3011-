
using backend.models;

namespace backend.repository
{
   /*
   
p   */
   public interface IBookRepository
    {
        
        Task AddBook(BookModel book);

        Task<BookModel?> GetBookById(int id);
        Task RemoveBook(int id);
        Task UpdateBook(BookModel book);

        Task UpdateStatus(int id);
        
    } 
}