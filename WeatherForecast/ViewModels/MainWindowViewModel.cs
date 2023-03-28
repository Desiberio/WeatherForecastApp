using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ReactiveUI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecastBackend;
using LiveCharts;
using LiveCharts.Wpf;
using Splat;
using System.Runtime.Remoting.Contexts;
using System.Reactive;
using System.Windows.Forms;
using WeatherForecastBackend.Models;
using WeatherForecastUI.Models;
using System.Windows.Media.Animation;
using System;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using WeatherForecastUI.Services;
using System.Configuration;
using System.Windows;

namespace WeatherForecastUI.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private WeatherForecastModel _weatherForecastModel;

        public WeatherForecastModel WeatherForecast
        {
            get { return _weatherForecastModel; }
            set => this.RaiseAndSetIfChanged(ref _weatherForecastModel, value);
        }

        public List<LocationModel> Locations { get; private set; }

        public IWeatherForecastService ForecastService { get; private set; }

        public MainWindowViewModel()
        {
            WeatherForecast = Locator.Current.GetService<IForecastDataModel>().CurrentWeatherForecast;
            Locations = Locator.Current.GetService<ILocationsProvider>().Locations;
            ForecastService = Locator.Current.GetService<IWeatherForecastService>();
            INotificationService notificationService = Locator.Current.GetService<INotificationService>();
            AutoUpdateService updateService = new AutoUpdateService(notificationService);

            ShowWindowCommand = ReactiveCommand.Create(() =>
            {
                App.Current.MainWindow.Show();
                App.Current.MainWindow.WindowState = WindowState.Normal;
            });
            GetCurrentForecastCommand = ReactiveCommand.Create(() =>
            {
                var notification = NotificationService.BuildNotification(new MessageModel(
                    "Погода",
                    $"{WeatherForecast.fact.factCondiotionRU}\nТемпература на данный момент {WeatherForecast.fact.tempFormatted}, " +
                    $"ощущается как {WeatherForecast.fact.feels_like}°C.\n{WeatherForecast.fact.windSummary}"));
                notificationService.ShowNotification(notification);
            });
            ExitCommand = ReactiveCommand.Create(() => App.Current.Shutdown());

            Locator.Current.GetService<IForecastDataModel>().ObservableForProperty(f => f.CurrentWeatherForecast)
                                                            .Subscribe(f => WeatherForecast = f.GetValue());
            notificationService.NotifyIfBadWeather(true);
        }

        public async Task ChangeLocation(LocationModel newLocation)
        {
            Locator.Current.GetService<IForecastDataModel>().CurrentWeatherForecast = await ForecastService.UpdateWeatherForecast(newLocation);
            
            Configuration config = Locator.Current.GetService<Configuration>();
            config.AppSettings.Settings["CurrentLocation"].Value = newLocation.City;
            config.Save();
        }

        public DoubleAnimation WrapTextBox { get; internal set; } = new DoubleAnimation()
        {
            From = 200,
            To = 20,
            Duration = TimeSpan.FromSeconds(0.35)
        };

        public DoubleAnimation UnwrapTextBox { get; internal set; } = new DoubleAnimation()
        {
            From = 20,
            To = 200,
            Duration = TimeSpan.FromSeconds(0.35)
        };
    
        public ReactiveCommand<Unit, Unit> ShowWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitCommand { get; }
        public ReactiveCommand<Unit, Unit> GetCurrentForecastCommand { get; }
    }
}
