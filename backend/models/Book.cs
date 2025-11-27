
using System.Collections.Generic;
namespace backend.models
{
    public class BookModel
    {
        public string BookId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public Genre BookGenre { get; set; }
       // public int numberofQueuedUsers { get; set; } = 0;

        public Queue<string> UserQueue { get; set; } = new Queue<string>();
        public MembershipLevel MembershipLevel { get; set; } = MembershipLevel.Standard;
        public BookStatus Status { get; set; } = BookStatus.Available;

        public BookModel(
            string bookId,
            string title,
            string author,
            string isbn,
            Genre genre
            ,
            MembershipLevel membershipLevel

        )
        {
            BookId = bookId;
            Title = title;
            Author = author;
            ISBN = isbn;
            BookGenre = genre;
            MembershipLevel = membershipLevel;

        }
        public MembershipLevel GetMembershipLevel()
        {
            return MembershipLevel;
        }

       
        

    }
}