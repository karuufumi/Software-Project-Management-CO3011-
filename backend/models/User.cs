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
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public Role UserRole { get; set; }

    public string? BorrowedBook { get; set; } = "";

    public BookQueue BookQueue { get; set; } = new BookQueue();

    public int MembershipPoints { get; set; } = 0;
    
    public UserModel(string id, string name, string email, Role role)
    {
        Id = id;
        Name = name;
        Email = email;
        UserRole = role;
    }


}


}