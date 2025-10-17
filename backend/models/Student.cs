
using backend.models;

namespace backend.models
{
    
    public class StudentModel(int id, string name, string email, string studentId) : UserModel(id, name, email, Role.Student)
    {
        public string StudentId { get; set; } = studentId;
    }

}