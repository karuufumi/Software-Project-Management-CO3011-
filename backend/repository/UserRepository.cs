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
            _dbSet = _context.Set<T>();
        }

        public async Task AddUser(T user)
        {
            await _dbSet.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<T?> GetUserById(string id)
        {
            // SPECIAL CASE: Student and Faculty need to load Membership
            if (typeof(T) == typeof(Student))
            {
                return await _context.Set<Student>()
                    .Include(s => s.MembershipPoints)
                    .FirstOrDefaultAsync(s => s.Id == id) as T;
            }

            if (typeof(T) == typeof(FacultyMember))
            {
                return await _context.Set<FacultyMember>()
                    .Include(f => f.MembershipPoints)
                    .FirstOrDefaultAsync(f => f.Id == id) as T;
            }

            // DEFAULT: No membership (Admin, Librarian)
            return await _dbSet.FindAsync(id);
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
                    throw new InvalidOperationException("User type does not have membership points.");
                }
            }
            throw new InvalidOperationException("User not found.");
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
                // Cast to dynamic to call increaseCreditScore if it exists
                if (user is Student student)
                {
                    student.increaseCreditScore(credits);
                }
                else if (user is FacultyMember faculty)
                {
                    faculty.increaseCreditScore(credits);
                }
                
                else
                {
                    throw new InvalidOperationException("User type does not support credit score modification.");
                }
                await _context.SaveChangesAsync();
                return user;
            }
            throw new InvalidOperationException("User not found.");
        }
        


    }
}
