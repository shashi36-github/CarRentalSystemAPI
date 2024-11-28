using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAPI.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Car make is required.")]
        public string Make { get; set; }

        [Required(ErrorMessage = "Car model is required.")]
        public string Model { get; set; }

        [Range(1900, 2100, ErrorMessage = "Please enter a valid year.")]
        public int Year { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price per day must be a positive value.")]
        public decimal PricePerDay { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}
