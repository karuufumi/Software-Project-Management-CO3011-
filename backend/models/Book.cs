
namespace backend.models
{
    public class BookModel
    {
        public int BookId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public Genre BookGenre { get; set; }

        // CRITICAL CHANGE: Replaced 'bool IsAvailable' with this Enum
        public BookStatus Status { get; set; } = BookStatus.Available;
    }
}