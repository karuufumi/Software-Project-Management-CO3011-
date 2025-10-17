
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
        public required string MemberId { get; set; }
        public  MembershipStatus Status { get; set; } = MembershipStatus.pending;

        public required string ApplicantId { get; set; }

        public required string RequestDate { get; set; } 

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
        
        public required string MembershipId { get; set; }
        public required string MemberName { get; set; }
        public required string StartDate { get; set; }
        public required string EndDate { get; set; }
        public required bool IsActive { get; set; }
        
        public required Tier MembershipTier { get; set; }  = Tier.Ngheo;

        

    }

    public enum Tier
    {
        Ngheo,
        LanDau,
        TuBan,
        VIP
    }

}