using backend.repository;
using backend.models;
using backend.Data;

namespace backend.repository
{
    public class MembershipRepository : IMembershipRepository
    {
        
        private readonly ApplicationDbContext _context;

        public MembershipRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddMembership(Membership membership)
        {
            await _context.Memberships.AddAsync(membership);
            await _context.SaveChangesAsync();
        }

        public async Task ExtendMembership(string userId, DateTime newExpiryDate)
        {
            var membership = await _context.Memberships.FindAsync(userId);
            if (membership != null)
            {
                membership.ExpiryDate = newExpiryDate;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ExtendMembership(string userId, int months)
        {
            var membership = await _context.Memberships.FindAsync(userId);
            if (membership != null)
            {
                membership.ExpiryDate = membership.ExpiryDate.AddMonths(months);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Membership?> GetMembershipByUserId(string userId)
        {
            return await _context.Memberships.FindAsync(userId);
        }

        public async Task RemoveMembership(string userId)
        {
            var membership = await _context.Memberships.FindAsync(userId);
            if (membership != null)
            {
                _context.Memberships.Remove(membership);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateMembership(Membership membership)
        {
            var existingMembership = await _context.Memberships.FindAsync(membership.UserId);
            if (existingMembership != null)
            {
                existingMembership.Level = membership.Level;
                existingMembership.ExpiryDate = membership.ExpiryDate;
                await _context.SaveChangesAsync();
            }
        }
    }
}