using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using DynamicData.Kernel;
using WeatherForecastUI.Models;

namespace WeatherForecastBackend
{
    public class LocationsProvider : ILocationsProvider
    {
        public List<LocationModel> Locations { get; internal set; }
        public LocationsProvider()
        {
            using (var reader = new StreamReader("./cities.csv"))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { MissingFieldFound = null, HeaderValidated = null}))
            {
                 Locations = csv.GetRecords<LocationModel>().ToList();
            }
        }
    }
}
