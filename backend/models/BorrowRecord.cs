using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.models
{
    public class BorrowRecord
    {
        [Key]
        public int Id { get; set; }

        // --- FOREIGN KEYS ---
        // These are just the numbers (e.g., 101, 5)
        public int BookId { get; set; }
        public int UserId { get; set; }

        // --- NAVIGATION PROPERTIES (Crucial!) ---
        // These allow EF to load the actual objects.
        // usage: var title = myRecord.Book.Title;
        public virtual required BookModel Book { get; set; }
        public virtual required UserModel User { get; set; }

        // --- TIMESTAMPS ---
        public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; } 
        
        // Null means "Currently Borrowed". 
        // If it has a date, it means "History/Returned".
        public DateTime? ReturnedAt { get; set; } 
    }
}