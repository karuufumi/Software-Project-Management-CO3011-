namespace backend.models 
{
    public class Librarian : UserModel
    {
        public string EmployeeId { get; set; } = string.Empty;
        
        
        public DateTime HireDate { get; set; } = DateTime.UtcNow;
        
        // Librarians don't have membership points
        // They manage borrowing for others
    }
}