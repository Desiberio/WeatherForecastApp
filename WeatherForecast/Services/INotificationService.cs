using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace WeatherForecastUI.Services
{
    public interface INotificationService
    {
        void ShowNotification(ToastNotification notification);
        void NotifyIfBadWeather(bool onStartup = false);
    }
}