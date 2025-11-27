
namespace backend.models
{
    public class BookModel
    {
        public int BookId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public Genre BookGenre { get; set; }

        public BookStatus Status { get; set; } = BookStatus.Available;
    }
}