using System.ComponentModel.DataAnnotations;

namespace backend.models
{
    public abstract class UserModel
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        public string Role { get; set; } = string.Empty; // "Student", "Faculty", "Librarian", "Admin"
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property for borrow records
        public ICollection<BorrowRecord>? BorrowedBooks { get; set; }
    }
}