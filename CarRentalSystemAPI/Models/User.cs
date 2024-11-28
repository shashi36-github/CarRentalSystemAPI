using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "User email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } // Note: In production, passwords should be hashed.

        [Required]
        [RegularExpression("Admin|User", ErrorMessage = "Role must be either 'Admin' or 'User'.")]
        public string Role { get; set; } = "User"; // "Admin" or "User"
    }
}
