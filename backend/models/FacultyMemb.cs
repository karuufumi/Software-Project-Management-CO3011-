using backend.models;
namespace backend.models
{
    
    class FacultyMembModel : UserModel
    {
        private string FacultyId { get; set; }

        public FacultyMembModel(int id, string name, string email, string facultyId)
            : base(id, name, email, Role.Member)
        {
            FacultyId = facultyId;
        }
    }
}

