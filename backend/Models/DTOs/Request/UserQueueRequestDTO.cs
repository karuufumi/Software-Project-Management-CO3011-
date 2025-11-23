using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTOs.Request
{
    public class UserQueueRequestDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int BookId { get; set; }
    }
}