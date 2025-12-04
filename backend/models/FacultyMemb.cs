namespace backend.models
{
    public class FacultyMember : UserModel
    {
        public string FacultyId { get; set; } = string.Empty;
        
        public int MembershipPoints { get; set; } = 0;
        
        public string Department { get; set; } = string.Empty;
        
        public string Position { get; set; } = string.Empty;

        public void increaseCreditScore(int points)
        {
            MembershipPoints += points;
        }
    }
}