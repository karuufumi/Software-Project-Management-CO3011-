using System.ComponentModel.DataAnnotations;
using backend.Models.Domain;

namespace backend.Models.DTOS.Response
{
    public class UserQueueResponseDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int BookId { get; set; }
        public User? User { get; set; }
        public Book? Book { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}