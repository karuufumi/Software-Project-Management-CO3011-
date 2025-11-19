

namespace backend.controllers 
 {
     
     public class BorrowRequest
     {
         public int BookId { get; set; }
         public int UserId { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }


        


     }


 }