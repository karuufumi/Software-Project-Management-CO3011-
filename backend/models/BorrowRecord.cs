using System.ComponentModel.DataAnnotations;

namespace backend.models
{
    public class BorrowRecord
    {
        [Key]
        public string BorrowId { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public string BookId { get; set; } = string.Empty;
        
        [Required]
        public DateTime BorrowDate { get; set; }
        
        // Set by librarian when approving request (null = pending request)
        public DateTime? DueDate { get; set; }
        
        // Set by librarian when user returns book (null = not returned yet)
        public DateTime? ReturnDate { get; set; }
        
        public bool IsReturned { get; set; } = false;
        
        // Navigation properties (optional)
        public UserModel? User { get; set; }
        public BookModel? Book { get; set; }
    }
}