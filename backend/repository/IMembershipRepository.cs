
using backend.models;

namespace backend.repository
{
    public interface IMembershipRepository
    {
        Task AddMembership( membership);

        Task<MembershipModel?> GetMembershipById(string id);
        Task RemoveMembership(string id);
        Task UpdateMembership(MembershipModel membership);
        
        Task ExtendMembership(string id, uint additionalMonths);
    }

}