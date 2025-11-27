using backend.models;


namespace backend.models 
{
    
    public class AdminModel : UserModel
    {
        private string AdminCode { get; set; }

        public AdminModel(int id, string name, string email, string adminCode)
            : base(id, name, email, Role.Admin)
        {
            AdminCode = adminCode;
        }
    }
}