using backend.models;
using Microsoft.EntityFrameworkCore;



namespace backend.repository
{
    public class UserRepository : IUserRepository
    {
       // protected readonly DbContext _context;
        protected readonly DbSet<UserModel> _users;

        private readonly Data.ApplicationDbContext _context;
        public UserRepository(Data.ApplicationDbContext context)
        {
            _context = context;
            _users = _context.Set<UserModel>();
        }

        public async Task AddUser(UserModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserModel?> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task RemoveUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUser(UserModel user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

    } 

}