using System.Threading.Tasks;
using WeatherForecastBackend.Models;
using WeatherForecastUI.Models;

namespace WeatherForecastBackend
{
    public interface IWeatherForecastService
    {
        PrecipitationMessageModel CheckPrecipitation(Forecast forecast);
        MessageModel CheckTemperature(Forecast firstDay, Forecast nextDay);
        Task<WeatherForecastModel> GetForecastAsync(double latitude, double longitude, string language = "ru_RU", int days = 7);
        Task<WeatherForecastModel> UpdateWeatherForecast(LocationModel location, bool forceUpdate = false);
    }
}