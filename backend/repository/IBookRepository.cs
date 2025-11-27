
using backend.models;

namespace backend.repository
{
   /*
   
p   */
   public interface IBookRepository
    {
        
        Task AddBook(BookModel book);

        Task<BookModel?> GetBookById(string id);
        Task RemoveBook(string id);
        Task UpdateBook(BookModel book);

        Task UpdateStatus(string id, int status);

    } 
}