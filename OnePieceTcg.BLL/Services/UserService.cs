using Microsoft.EntityFrameworkCore;
using OnePieceTcg.DAL.Data;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.BLL.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        }
        public async Task<bool> AddUserAsync(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return false;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
            {

            }
        }
    }
}
