using backend.models;

public enum Tier
{
    Tier_1,
    Tier_2,
    Tier_3
    ,
    Tier_4
    ,
    Tier_5
}

namespace backend.models 
{
    public class Membership
    {
        public required int MembershipId { get; set; }      // PK

        public required int UserId { get; set; }           // FK to UserModel

        public required Tier MembershipTier { get; set; }

        public required DateTime ExpiryDate { get; set; }

        // Navigation property
        public UserModel? User { get; set; }
    }
}