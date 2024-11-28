using CarRentalSystemAPI.Models;
using CarRentalSystemAPI.Repositories;
using CarRentalSystemAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarRentalSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly ICarRentalService _carRentalService;
        private readonly ILogger<CarController> _logger;

        public CarController(
            ICarRepository carRepository,
            ICarRentalService carRentalService,
            ILogger<CarController> logger)
        {
            _carRepository = carRepository;
            _carRentalService = carRentalService;
            _logger = logger;
        }

        // GET /api/cars
        /// <summary>
        /// Retrieves a list of all available cars.
        /// </summary>
        [HttpGet]
        public IActionResult GetAvailableCars()
        {
            _logger.LogInformation("Fetching all available cars.");
            var cars = _carRepository.GetAvailableCars();
            return Ok(cars);
        }

        // GET /api/cars/{id}
        /// <summary>
        /// Retrieves details of a specific car by ID.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetCarById(int id)
        {
            _logger.LogInformation("Fetching car details for ID: {CarId}", id);
            var car = _carRepository.GetCarById(id);
            if (car == null)
            {
                _logger.LogWarning("Car with ID: {CarId} not found.", id);
                return NotFound();
            }
            return Ok(car);
        }

        // POST /api/cars
        /// <summary>
        /// Adds a new car to the fleet. (Admin only)
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public IActionResult AddCar([FromBody] Car car)
        {
            _logger.LogInformation("Attempting to add a new car.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid car data received.");
                return BadRequest(ModelState);
            }

            _carRepository.AddCar(car);
            _logger.LogInformation("Car added successfully with ID: {CarId}", car.Id);

            return CreatedAtAction(nameof(GetCarById), new { id = car.Id }, car);
        }

        // PUT /api/cars/{id}
        /// <summary>
        /// Updates car details and availability. (Admin only)
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public IActionResult UpdateCar(int id, [FromBody] Car car)
        {
            _logger.LogInformation("Attempting to update car with ID: {CarId}", id);

            if (id != car.Id)
            {
                _logger.LogWarning("Car ID mismatch. URL ID: {UrlId}, Body ID: {BodyId}", id, car.Id);
                return BadRequest("Car ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid car data received for update.");
                return BadRequest(ModelState);
            }

            var existingCar = _carRepository.GetCarById(id);
            if (existingCar == null)
            {
                _logger.LogWarning("Car with ID: {CarId} not found for update.", id);
                return NotFound();
            }

            // Update the existing car details
            existingCar.Make = car.Make;
            existingCar.Model = car.Model;
            existingCar.Year = car.Year;
            existingCar.PricePerDay = car.PricePerDay;
            existingCar.IsAvailable = car.IsAvailable;

            _carRepository.UpdateCar(existingCar);
            _logger.LogInformation("Car with ID: {CarId} updated successfully.", id);

            return NoContent();
        }

        // DELETE /api/cars/{id}
        /// <summary>
        /// Deletes a car from the fleet. (Admin only)
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public IActionResult DeleteCar(int id)
        {
            _logger.LogInformation("Attempting to delete car with ID: {CarId}", id);

            var car = _carRepository.GetCarById(id);
            if (car == null)
            {
                _logger.LogWarning("Car with ID: {CarId} not found for deletion.", id);
                return NotFound();
            }

            _carRepository.DeleteCar(id);
            _logger.LogInformation("Car with ID: {CarId} deleted successfully.", id);

            return NoContent();
        }

        // POST /api/cars/{id}/rent
        /// <summary>
        /// Rents a car to the authenticated user.
        /// </summary>
        [Authorize]
        [HttpPost("{id}/rent")]
        public async Task<IActionResult> RentCar(int id)
        {
            _logger.LogInformation("User attempting to rent car with ID: {CarId}", id);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger.LogWarning("User ID claim not found.");
                return Unauthorized();
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                _logger.LogWarning("Invalid user ID.");
                return Unauthorized();
            }

            try
            {
                if (!_carRentalService.CheckCarAvailability(id))
                {
                    _logger.LogWarning("Car with ID: {CarId} is not available.", id);
                    return BadRequest("Car is not available.");
                }

                await _carRentalService.RentCar(id, userId);
                _logger.LogInformation("Car with ID: {CarId} rented successfully by user ID: {UserId}", id, userId);

                return Ok("Car rented successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error renting car with ID: {CarId} by user ID: {UserId}", id, userId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
