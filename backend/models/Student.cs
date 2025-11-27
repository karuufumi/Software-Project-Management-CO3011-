
using backend.models;

namespace backend.models
{

    public class Student(string id, string name, string email, string studentId) : UserModel(id, name, email, Role.Student)
    {
        private BookQueue bookQueue = new BookQueue(id);
        
        private UInt16 CreditScore { get; set; } = 0;
        public void increaseCreditScore(int points) => CreditScore += (UInt16)points;

        private MembershipLevel Membership{ get; set; } = MembershipLevel.Standard;
        public string StudentId { get; set; } = studentId;
    }

}