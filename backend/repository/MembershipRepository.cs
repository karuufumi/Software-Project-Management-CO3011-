using backend.models;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.repository
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Membership> _memberships;

        public MembershipRepository(ApplicationDbContext context)
        {
            _context = context;
            _memberships = context.Memberships;   // ✔ Correct
        }

        public async Task AddMembership(Membership membership)
        {
            await _memberships.AddAsync(membership); // ✔ Works now
            await _context.SaveChangesAsync();
        }

        public async Task<Membership?> GetMembershipById(int id)
        {
            return await _memberships
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MembershipId == id);
        }

        public async Task CancelMembership(int id)
        {
            var membership = await _memberships.FindAsync(id);
            if (membership == null) return;

            membership.IsActive = false;
            membership.EndDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

            await _context.SaveChangesAsync();
        }

        public async Task UpdateMembership(Membership membership)
        {
            _memberships.Update(membership);
            await _context.SaveChangesAsync();
        }

        public async Task ExtendMembership(int id, DateTime newExpiryDate)
        {
            var membership = await _memberships.FindAsync(id);
            if (membership == null) return;

            membership.EndDate = newExpiryDate.ToString("yyyy-MM-dd");
            membership.IsActive = true;

            await _context.SaveChangesAsync();
        }
    }
}
