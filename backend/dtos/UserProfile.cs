using backend.models;


namespace backend.dtos
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public BookModel BorrowedBook { get; set; }
        public Queue BorrowList { get; set; }

        public int MembershipPoints { get; set; } = 0;



        public UserProfile(UserModel user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            BorrowedBook = user.BorrowedBook!;
            BorrowList = user.BorrowList;

        }
        
    }
}