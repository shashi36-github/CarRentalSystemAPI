using System.Threading.Tasks;

namespace CarRentalSystemAPI.Services
{
    public interface INotificationService
    {
        Task SendBookingConfirmationEmail(string toEmail, string subject, string message);
    }
}
