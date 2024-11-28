using System.Threading.Tasks;

namespace CarRentalSystemAPI.Services
{
    public interface ICarRentalService
    {
        Task RentCar(int carId, int userId);
        bool CheckCarAvailability(int carId);
    }
}
