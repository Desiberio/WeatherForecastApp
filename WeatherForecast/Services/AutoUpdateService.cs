using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WeatherForecastBackend;
using WeatherForecastBackend.Models;
using WeatherForecastUI.Models;
using WeatherForecastUI.ViewModels;

namespace WeatherForecastUI.Services
{
    public class AutoUpdateService
    {
        private readonly INotificationService _notificationService;
        private IWeatherForecastService _forecastService;
        Timer _timer;
        public AutoUpdateService(INotificationService notificationService)
        {
            _forecastService =  Locator.Current.GetService<IWeatherForecastService>();
            _timer = new Timer(TimeSpan.FromHours(3).TotalMilliseconds);
            _timer.Elapsed += UpdateWeatherForecast;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _notificationService = notificationService;
        }

        private void UpdateWeatherForecast(object sender, ElapsedEventArgs e)
        {
            IForecastDataModel data = Locator.Current.GetService<IForecastDataModel>();
            data.CurrentWeatherForecast = _forecastService.UpdateWeatherForecast(data.CurrentLocation, true).Result;
            _notificationService.NotifyIfBadWeather();
        }
    }
}
