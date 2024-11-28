using CarRentalSystemAPI.Models;
using System.Collections.Generic;

namespace CarRentalSystemAPI.Repositories
{
    public interface ICarRepository
    {
        void AddCar(Car car);
        Car GetCarById(int id);
        IEnumerable<Car> GetAvailableCars();
        void UpdateCar(Car car);
        void DeleteCar(int id);
    }
}
