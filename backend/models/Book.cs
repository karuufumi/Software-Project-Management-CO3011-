using System.ComponentModel.DataAnnotations;

namespace backend.models
{
    public class BookModel
    {
        [Key]
        public string BookId { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        public string Author { get; set; } = string.Empty;
        
        public string ISBN { get; set; } = string.Empty;
        
        public string Genre { get; set; } = string.Empty;
        
        public DateTime PublishedDate { get; set; }
        
        [Required]
        public int TotalCopies { get; set; }
        
        [Required]
        public int AvailableCopies { get; set; }
        
        public string? Description { get; set; }
        
        // Navigation property for borrow records
        public ICollection<BorrowRecord>? BorrowRecords { get; set; }
    }
}