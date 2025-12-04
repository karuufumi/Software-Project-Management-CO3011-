using backend.Data;
using backend.models;
using Microsoft.EntityFrameworkCore;

namespace backend.repository
{
    public class UserRepository<T> : IUserRepository<T> where T : UserModel
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddUser(T user)
        {
            await _dbSet.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<T?> GetUserById(string id)
        {
            if (typeof(T) == typeof(Student))
            {
                return await _context.Students
                    .Include(s => s.BorrowedBooks)
                    .FirstOrDefaultAsync(s => s.Id == id) as T;
            }

            if (typeof(T) == typeof(FacultyMember))
            {
                return await _context.FacultyMembers
                    .Include(f => f.BorrowedBooks)
                    .FirstOrDefaultAsync(f => f.Id == id) as T;
            }

            return await _dbSet
                .Include(u => u.BorrowedBooks)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllUsers()
        {
            return await _dbSet
                .Include(u => u.BorrowedBooks)
                .ToListAsync();
        }

        public async Task<int> GetUserMembership(string userId)
        {
            var user = await GetUserById(userId);
            if (user != null)
            {
                if (user is Student student)
                {
                    return student.MembershipPoints;
                }
                else if (user is FacultyMember faculty)
                {
                    return faculty.MembershipPoints;
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }

        public async Task RemoveUser(string id)
        {
            var user = await GetUserById(id);
            if (user != null)
            {
                _dbSet.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUser(T user)
        {
            _dbSet.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<T?> UpdateCredit(string userId, int credits)
        {
            var user = await GetUserById(userId);
            if (user != null)
            {
                if (user is Student student)
                {
                    student.increaseCreditScore(credits);
                }
                else if (user is FacultyMember faculty)
                {
                    faculty.increaseCreditScore(credits);
                }
                
                await _context.SaveChangesAsync();
                return user;
            }
            return null;
        }
    }
}