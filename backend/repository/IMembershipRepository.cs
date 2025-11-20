
using backend.models;

namespace backend.repository
{
    public interface IMembershipRepository
    {
        
        Task AddMembership(Membership membership);

        Task<Membership?> GetMembershipById(int id);
        Task  CancelMembership(int id);
        Task UpdateMembership(Membership membership);

        Task ExtendMembership(int id, DateTime newExpiryDate);


    }
}
