using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecastBackend.Models
{
    public class MessageModel
    {
        public string Header { get; internal set; }
        public string Body { get; internal set; }

        public MessageModel(string header, string body)
        {
            Header = header;
            Body = body;
        }
    }
}
