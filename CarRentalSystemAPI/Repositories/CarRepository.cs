using CarRentalSystemAPI.Data;
using CarRentalSystemAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CarRentalSystemAPI.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarRentalDbContext _context;

        public CarRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public void AddCar(Car car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
        }

        public Car GetCarById(int id)
        {
            return _context.Cars.Find(id);
        }

        public IEnumerable<Car> GetAvailableCars()
        {
            return _context.Cars.Where(c => c.IsAvailable).ToList();
        }

        public void UpdateCar(Car car)
        {
            _context.Entry(car).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteCar(int id)
        {
            var car = GetCarById(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }
        }
    }
}
