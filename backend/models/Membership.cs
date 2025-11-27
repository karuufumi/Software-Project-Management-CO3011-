
using backend.models;


namespace backend.models
{
    public class Membership
    {
        

        public string UserId { get; set; }
        
        public DateTime ExpiryDate { get; set; }

        public MembershipLevel Level { get; set; }

        public Membership(string userId, DateTime expiryDate, MembershipLevel level)
        {
            UserId = userId;
            ExpiryDate = expiryDate;
            Level = level;
        }

    }
}