using backend.models;
namespace backend.models
{
    
    class FacultyMember : UserModel
    {
        private string FacultyId { get; set; }
        private  BookQueue bookQueue = new BookQueue();
        private UInt16 CreditScore { get; set; } = 0;

        private MembershipLevel Membership { get; set; } = MembershipLevel.Standard;

        public void increaseCreditScore(int points) => CreditScore += (UInt16)points;
        public FacultyMember(string id, string name, string email, string facultyId)
            : base(id, name, email, Role.Member)
        {
            FacultyId = facultyId;
        }
    }
}

