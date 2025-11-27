namespace backend.models
{

public enum Role
{
    Admin,
    Student,
    Member,
    Librarian,



}

public abstract class UserModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public Role UserRole { get; set; }
    
    public BookModel? BorrowedBook { get; set; }
    public Queue BorrowList { get; set; } = default!;

    public int MembershipPoints { get; set; } = 0;
    
    public UserModel(int id, string name, string email, Role role)
    {
        Id = id;
        Name = name;
        Email = email;
        UserRole = role;
    }


}


}