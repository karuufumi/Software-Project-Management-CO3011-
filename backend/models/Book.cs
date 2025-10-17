using System.Net;
using backend.models;

public enum Genre
{
    Fiction,
    NonFiction,
    Science,
    History,
    Biography,
    Children,
    Fantasy,
    Mystery,
    Engineering
}


namespace backend.models 
{
    public class BookModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        
        public Genre BookGenre { get; set; }

        public bool IsAvailable { get; set; } = true;
        public BookModel(int bookId, string title, string author, string isbn, bool isAvailable = true, Genre Bookgenre = Genre.Fiction)
        {
            BookId = bookId;
            Title = title;
            Author = author;
            ISBN = isbn;
            isAvailable = IsAvailable;
            BookGenre = Bookgenre;
        }
    }
    
   }
