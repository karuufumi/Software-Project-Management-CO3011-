using backend.Data;
using backend.models;
using Microsoft.EntityFrameworkCore;

namespace backend.repository
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly ApplicationDbContext _context;

        public MembershipRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Membership?> GetMembershipByUserId(string userId)
        {
            return await _context.Memberships
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UserId == userId);
        }

        public async Task AddMembership(Membership membership)
        {
            await _context.Memberships.AddAsync(membership);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMembership(Membership membership)
        {
            membership.LastUpdated = DateTime.UtcNow;
            _context.Memberships.Update(membership);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMembership(string userId)
        {
            var membership = await GetMembershipByUserId(userId);
            if (membership != null)
            {
                _context.Memberships.Remove(membership);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ExtendMembership(string userId, DateTime newExpiryDate)
        {
            var membership = await GetMembershipByUserId(userId);
            if (membership == null)
            {
                throw new InvalidOperationException("Membership not found.");
            }

            membership.ExpiryDate = newExpiryDate;
            await UpdateMembership(membership);
        }

        public async Task ExtendMembership(string userId, int months)
        {
            var membership = await GetMembershipByUserId(userId);
            if (membership == null)
            {
                throw new InvalidOperationException("Membership not found.");
            }

            membership.ExpiryDate = membership.ExpiryDate.AddMonths(months);
            await UpdateMembership(membership);
        }

        public async Task<bool> IsMembershipActive(string userId)
        {
            var membership = await GetMembershipByUserId(userId);
            return membership != null && membership.ExpiryDate > DateTime.UtcNow;
        }

        public async Task AddPoints(string userId, int points)
        {
            var membership = await GetMembershipByUserId(userId);
            if (membership == null)
            {
                throw new InvalidOperationException("Membership not found.");
            }

            membership.Points += points;
            await UpdateMembership(membership);
        }

        public async Task DeductPoints(string userId, int points)
        {
            var membership = await GetMembershipByUserId(userId);
            if (membership == null)
            {
                throw new InvalidOperationException("Membership not found.");
            }

            if (membership.Points < points)
            {
                throw new InvalidOperationException("Insufficient membership points.");
            }

            membership.Points -= points;
            await UpdateMembership(membership);
        }
    }
}