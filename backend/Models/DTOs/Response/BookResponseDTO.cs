using backend.Models.Domain;

namespace backend.Models.DTOS.Response
{
    public class BookResponseDTO
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
    }
}