using backend.models;
using backend.Data;
using Microsoft.EntityFrameworkCore;



namespace backend.repository
{
    
    public class MembershipRepository : IMembershipRepository
    {
        protected readonly DbSet<Membership> _memberships;

        private readonly ApplicationDbContext _context;
        public MembershipRepository(ApplicationDbContext context)
        {
            _context = context;
            _memberships = _context.Set<Membership>();
        }

        public async Task AddMembership(Membership membership)
        {
            //_context..Add(membership);
            _context.Memberships.Add(membership);
            await _context.SaveChangesAsync();
        }

        public async Task<Membership?> GetMembershipById(int id)
        {
            return await _context.Memberships.FindAsync(id);
        }

        public async Task CancelMembership(int id)
        {
            var membership = await _context.Memberships.FindAsync(id);
            if (membership != null)
            {
                _context.Memberships.Remove(membership);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateMembership(Membership membership)
        {
            _context.Memberships.Update(membership);
            await _context.SaveChangesAsync();
        }

        public async Task ExtendMembership(int id, DateTime newExpiryDate)
        {
            var membership = await _context.Memberships.FindAsync(id);
            if (membership != null)
            {
                membership.EndDate = newExpiryDate.ToString("yyyy-MM-dd");
                _context.Memberships.Update(membership);
                await _context.SaveChangesAsync();
            }
        }
    }

}