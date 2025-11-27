using backend.models;

namespace backend.models
{
    public class Queue
    {
        public required int QueueId { get; set; }          // PK

        // FK to BookModel
//        public required int BookId { get; set; }           // FK
        
        public required int UserId {get; set;}


        public List<BookModel> bookList { get; set; } = new List<BookModel>();
        



    }
}