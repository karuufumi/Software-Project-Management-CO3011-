namespace backend.models 
{
    public class Admin : UserModel
    {
        public string AdminId { get; set; } = string.Empty;
        
        public string AccessLevel { get; set; } = "SuperAdmin"; // e.g., "SuperAdmin", "SystemAdmin"
        
        public DateTime LastLogin { get; set; }
        
        // Admins don't have membership points
        // They have system-wide permissions
    }
}