using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.Domain
{
    public class Book
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
    }
}