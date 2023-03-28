using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherForecastBackend.Models;

namespace WeatherForecastUI.Models
{
    public interface IForecastDataModel
    {
        WeatherForecastModel CurrentWeatherForecast { get; set; }
        LocationModel CurrentLocation { get; set; }
        Forecast SelectedForecast { get; set; }
    }
}