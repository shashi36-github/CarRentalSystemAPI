using CarRentalSystemAPI.Data;
using CarRentalSystemAPI.Models;
using System.Linq;

namespace CarRentalSystemAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CarRentalDbContext _context;

        public UserRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }
    }
}
