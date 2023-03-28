using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecastBackend;
using WeatherForecastBackend.Models;
using ReactiveUI.Wpf;
using ReactiveUI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using DynamicData;

namespace WeatherForecastUI.Models
{
    public class ForecastDataModel : ReactiveObject, IForecastDataModel
    {
        private WeatherForecastModel _currentWeatherForecast;
        private Forecast _selectedForecast;
        public LocationModel CurrentLocation { get; set; }

        public WeatherForecastModel CurrentWeatherForecast
        {
            get { return _currentWeatherForecast; }
            set
            {
                this.RaiseAndSetIfChanged(ref _currentWeatherForecast, value);
                SelectedForecast = CurrentWeatherForecast.forecasts[0];
            }
        }

        public Forecast SelectedForecast
        {
            get { return _selectedForecast; }
            set => this.RaiseAndSetIfChanged(ref _selectedForecast, value);
        }

        public ForecastDataModel(IWeatherForecastService service, LocationModel location)
        {
            CurrentLocation = location;
            CurrentWeatherForecast = service.GetForecastAsync(CurrentLocation.Lat, CurrentLocation.Lng).Result;
        }
    }
}
