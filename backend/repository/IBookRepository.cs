
using backend.models;

namespace backend.repository
{
   /*
   
public BookModel(int bookId, string title, string author, string isbn, bool isAvailable = true, Genre Bookgenre = Genre.Fiction)
        {
            BookId = bookId;
            Title = title;
            Author = author;
            ISBN = isbn;
            isAvailable = IsAvailable;
            BookGenre = Bookgenre;
        }

   */
   public interface IBookRepository
    {
        
        Task AddBook(BookModel book);

        Task<BookModel?> GetBookById(int id);
        Task RemoveBook(int id);
        Task UpdateBook(BookModel book);

        Task UpdateStatus(int id);
        
    } 
}