namespace backend.models
{
    public class Student : UserModel
    {
        public string StudentId { get; set; } = string.Empty;
        
        public int MembershipPoints { get; set; } = 0;
        
        public string Major { get; set; } = string.Empty;
        
        public int Year { get; set; }

        public void increaseCreditScore(int points)
        {
            MembershipPoints += points;
        }
    }
}