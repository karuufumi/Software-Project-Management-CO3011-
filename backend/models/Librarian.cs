using backend.models;

namespace backend.models 
{
    
    public class LibrarianModel : UserModel
    {
        public string EmployeeId { get; set; }

        public LibrarianModel(string id, string name, string email, string librarianId)
            : base(id, name, email, Role.Librarian)
        {
            EmployeeId = librarianId;
        }
    }
}