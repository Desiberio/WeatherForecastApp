using System.Collections.Generic;
using WeatherForecastUI.Models;

namespace WeatherForecastBackend
{
    public interface ILocationsProvider
    {
        List<LocationModel> Locations { get; }
    }
}