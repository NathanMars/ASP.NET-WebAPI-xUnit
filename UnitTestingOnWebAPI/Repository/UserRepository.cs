using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class UserRepository(UserDbContext context) : IUserInterface
    {
        public async Task<bool> CreateAsync(User user)
        {
            context!.Users.Add(user);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<User> GetByIdAsync(int id) => await context!.Users!.FirstOrDefaultAsync(user => user.Id == id);

        public async Task<IEnumerable<User>> GetAllAsync() => await context!.Users.ToListAsync();

        public async Task<bool> UpdateAsync(User user)
        {
            var getUser = await context!.Users.FirstOrDefaultAsync(user => user.Id == user.Id);
            if (getUser != null)
            {
                getUser.Name = user.Name;
                getUser.Email = user.Email;
                var result = await context.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var getUser = await context.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (getUser != null)
            {
                context.Users.Remove(getUser);
                var result = await context.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }
    }
}
