using CarRentalSystemAPI.Repositories;
using System;
using System.Threading.Tasks;
using CarRentalSystemAPI.Models;

namespace CarRentalSystemAPI.Services
{
    public class CarRentalService : ICarRentalService
    {
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public CarRentalService(ICarRepository carRepository, IUserRepository userRepository, INotificationService notificationService)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
        }

        public bool CheckCarAvailability(int carId)
        {
            var car = _carRepository.GetCarById(carId);
            return car != null && car.IsAvailable;
        }

        public async Task RentCar(int carId, int userId)
        {
            var car = _carRepository.GetCarById(carId);
            var user = _userRepository.GetUserById(userId);

            if (car == null)
            {
                throw new Exception("Car not found.");
            }

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (!car.IsAvailable)
            {
                throw new Exception("Car is not available for rental.");
            }

            // Update car availability
            car.IsAvailable = false;
            _carRepository.UpdateCar(car);

            // Send notification
            string subject = "Car Booking Confirmation";
            string message = $"Dear {user.Name},\n\nYour booking for {car.Make} {car.Model} ({car.Year}) has been confirmed.\n\nThank you for choosing our service!";
            await _notificationService.SendBookingConfirmationEmail(user.Email, subject, message);
        }
    }
}
