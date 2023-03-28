using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecastUI.Models
{
    public class LocationModel
    {
        public string RegionType { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string FullAddress { get; set; }
    }
}
