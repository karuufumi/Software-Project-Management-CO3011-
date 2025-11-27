
using backend.models;


namespace backend.models
{
    public enum MembershipStatus
    {
       pending,
       approve,
       rejected,
    }
    
    public class MembershipRequest
    {
        public  string MemberId { get; set; }
        public  MembershipStatus Status { get; set; } = MembershipStatus.pending;

        public  string ApplicantId { get; set; }

        public  string RequestDate { get; set; } 

        public MembershipRequest(string memberId, string applicantId, string requestDate, MembershipStatus status = MembershipStatus.pending)
        {
            MemberId = memberId;
            ApplicantId = applicantId;
            RequestDate = requestDate;
            Status = status;
        }

    }


    public class Membership
    {
        public required int MembershipId { get; set; }   // PK

        // FK to User
        public required int UserId { get; set; }         
        public required UserModel User { get; set; }     // Navigation property

        public string StartDate { get; set; } = default!;
        public string EndDate { get; set; } = default!;
        public bool IsActive { get; set; }

        public Tier MembershipTier { get; set; } = Tier.Ngheo;
    }
}


    public enum Tier
    {
        Ngheo,
        LanDau,
        TuBan,
        VIP
    }
