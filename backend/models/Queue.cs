using System.ComponentModel.DataAnnotations;

namespace backend.models
{
    public class BookQueue
    {
        [Key]
        public string QueueId { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        // Navigation property for queued books
        public ICollection<BookModel>? QueuedBooks { get; set; }
        
        // Navigation property to user
        public UserModel? User { get; set; }
    }
}