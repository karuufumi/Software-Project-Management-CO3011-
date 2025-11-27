
namespace backend.models
{
    public class BorrowRequest
    {
        public int Id { get; set; }

        // Relationships
        public int BookId { get; set; }
        public virtual required BookModel Book { get; set; }

        public int UserId { get; set; } // Assuming User ID is int based on your Context
        public virtual required UserModel User { get; set; }

        // Queue Logic
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public RequestStatus Status { get; set; } = RequestStatus.Waiting;
        
        // Only set when Status becomes 'Allocated'
        public DateTime? AllocationExpiry { get; set; } 
    }
}