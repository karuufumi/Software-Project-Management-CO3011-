
using backend.models;

namespace backend.repository
{
    
    public interface IMembershipRepository
    {
        Task<Membership?> GetMembershipByUserId(string userId);
        Task AddMembership(Membership membership);
        Task UpdateMembership(Membership membership);
        Task RemoveMembership(string userId);

    
        Task ExtendMembership(string userId, DateTime newExpiryDate);
        Task ExtendMembership(string userId, int months);
    }
}