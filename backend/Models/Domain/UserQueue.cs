namespace backend.Models.Domain
{
    public class UserQueue
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public required User User { get; set; }
        public required Book Book { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}