using System.ComponentModel.DataAnnotations;

namespace backend.models
{
    public class Membership
    {
        [Key]
        public string MembershipId { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public int Points { get; set; } = 0;
        
        [Required]
        public DateTime ExpiryDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastUpdated { get; set; }
        
        // Navigation property
        public UserModel? User { get; set; }
    }
}