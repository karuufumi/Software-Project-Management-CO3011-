namespace backend.models
{
    public enum Genre
    {
        Fiction, NonFiction, Science, History, Biography, 
        Children, Fantasy, Mystery, Engineering
    }

    public enum BookStatus
    {
        Available,      // Free for anyone
        Borrowed,       // Currently out with a user
        Lost,           // Gone forever
    }

    public enum RequestStatus
    {
        Waiting,    // Still in line
        Allocated,  // It is their turn (User notified)
        Fulfilled,  // They picked it up (Moved to BorrowRecord)
        Expired,    // They missed the deadline
        Canceled    // User left the line
    }

    public enum MembershipLevel
    {
        Standard,
        Premium,
        VIP
    }
}