using backend.models;

namespace backend.models
{
    public class BorrowReqModel
    {
        public string UserId { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public DateTime ReturnDate { get; set; } = DateTime.Now.AddDays(7);


        public BorrowReqModel(string userId, string bookId, DateTime requestDate, DateTime returnDate)
        {
            UserId = userId;
            BookId = bookId;
            RequestDate = requestDate;
            ReturnDate = returnDate;
        }
        
    }

}