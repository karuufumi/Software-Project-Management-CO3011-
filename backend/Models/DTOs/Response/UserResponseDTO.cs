using System.ComponentModel.DataAnnotations;
using backend.Models.Domain;

namespace backend.Models.DTOS.Response
{
    public class UserResponseDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}