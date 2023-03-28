using Splat;
using System;
using System.Threading.Tasks;
using WeatherForecastBackend;
using WeatherForecastBackend.Models;
using WeatherForecastUI.Models;
using WeatherForecastUI.ViewModels;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace WeatherForecastUI.Services
{
    public class NotificationService : INotificationService
    {
        private IWeatherForecastService _forecastService;
        private PrecipitationMessageModel lastPrecipitationMessage = null;

        public NotificationService(string applicationId)
        {
            _forecastService = Locator.Current.GetService<IWeatherForecastService>();
        }

        public void ShowNotification(ToastNotification notification)
        {
            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier("Weather Forecast");
            toastNotifier.Show(notification);
        }

        public async void NotifyIfBadWeather(bool onStartup = false)
        {
            //delaying notifications to avoid situations where user boots his computer and goes away while its booting
            //the better way of doing this is to capture mouse to analyze is user active right now so he will definitely see notification
            //but this approach requires more rights to start application
            if (onStartup) await Task.Delay(new TimeSpan(0,15, 0));

            Forecast today = Locator.Current.GetService<IForecastDataModel>().CurrentWeatherForecast.forecasts[0];
            Forecast tomorrow = Locator.Current.GetService<IForecastDataModel>().CurrentWeatherForecast.forecasts[1];

            var precipitationMessage = _forecastService.CheckPrecipitation(today);
            precipitationMessage = CompareToLastMessage(precipitationMessage);
            var temperatureMessage = _forecastService.CheckTemperature(today, tomorrow);

            if (precipitationMessage != null) ShowNotification(BuildNotification(precipitationMessage));
            if (temperatureMessage != null) ShowNotification(BuildNotification(temperatureMessage));
        }

        private PrecipitationMessageModel CompareToLastMessage(PrecipitationMessageModel precipitationMessage)
        {
            if (lastPrecipitationMessage == null) lastPrecipitationMessage = precipitationMessage;
            else
            {
                if (lastPrecipitationMessage.PrecipitationStart.Hour == precipitationMessage.PrecipitationStart.Hour &&
                    lastPrecipitationMessage.PrecipitationEnd.Hour == precipitationMessage.PrecipitationEnd.Hour)
                {
                    precipitationMessage = null;
                }
                else lastPrecipitationMessage = precipitationMessage;
            }

            return precipitationMessage;
        }

        public static ToastNotification BuildNotification(MessageModel message)
        {
            var xml = $@"<toast>
                            <visual>
                                <binding template=""ToastText02"">
                                    <text id=""1"">{message.Header}</text>
                                    <text id=""2"">{message.Body}</text>
                                </binding>
                            </visual>
                        </toast>";
            var toastXml = new XmlDocument();
            toastXml.LoadXml(xml);
            var toast = new ToastNotification(toastXml);
            return toast;
        }
    }
}