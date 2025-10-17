using backend.models;

namespace backend.models 
{
    
    class LibrarianModel : UserModel
    {
        private string EmployeeId { get; set; }

        public LibrarianModel(int id, string name, string email, string librarianId)
            : base(id, name, email, Role.Librarian)
        {
            EmployeeId = librarianId;
        }
    }
}