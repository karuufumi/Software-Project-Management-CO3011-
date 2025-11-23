using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTOs.Request
{
    public class UpdateBookRequestDTO
    {
        [Required]
        [Range(1, 100000)]
        public decimal Quantity { get; set; }
    }
}