using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecastBackend.Models
{
    public class PrecipitationMessageModel : MessageModel
    {
        public DateTime PrecipitationStart { get; private set; }
        public DateTime PrecipitationEnd { get; private set; }
        public PrecipitationMessageModel(string header, string body, DateTime precipitationStart, DateTime precipitationEnd) : base(header, body)
        {
            Header = header;
            Body = body;
            PrecipitationStart = precipitationStart;
            PrecipitationEnd = precipitationEnd;
        }
    }
}
