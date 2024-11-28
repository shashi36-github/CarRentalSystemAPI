using CarRentalSystemAPI.Models;

namespace CarRentalSystemAPI.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User GetUserByEmail(string email);
        User GetUserById(int id);
    }
}
