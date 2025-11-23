using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTOs.Request
{
    public class CreateUserRequestDTO
    {
        [Required]
        [MaxLength(280, ErrorMessage = "Username length should not be over 280")]
        public string Username { get; set; } = string.Empty;
        public string? Role { get; set; }
    }
}