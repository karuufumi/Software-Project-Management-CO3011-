using backend.models;

namespace backend.models
{
    public class BookQueue
    {
        /*
        public required int QueueId { get; set; }          // PK

        // FK to BookModel
//        public required int BookId { get; set; }           // FK

        public required int UserId { get; set; }

        public List<BookModel> bookList { get; set; } = new List<BookModel>();
        */
        public string QueueId { get; set; } = string.Empty;

        public List<BookModel> bookList { get; set; } = new List<BookModel>();
        

        //Constructor

        public BookQueue()
        {
        }
        public BookQueue(string queueId)
        {
            QueueId = queueId;
        }



        

    }
}