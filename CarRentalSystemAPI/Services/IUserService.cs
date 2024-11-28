using CarRentalSystemAPI.Models;

namespace CarRentalSystemAPI.Services
{
    public interface IUserService
    {
        void RegisterUser(User user);
        string AuthenticateUser(string email, string password);
    }
}
