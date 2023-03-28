using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WeatherForecastBackend.Models;
using WeatherForecastUI.Models;

namespace WeatherForecastBackend
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly HttpClient _client;
        private WeatherForecastModel _weatherForecast;
        private List<WeatherForecastModel> _weatherForecasts = new List<WeatherForecastModel>();


        public WeatherForecastService(string token)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("X-Yandex-API-Key", token);
        }

        public async Task<WeatherForecastModel> GetForecastAsync(double latitude, double longitude, string language = "ru_RU", int days = 7)
        {
            string url = $"https://api.weather.yandex.ru/v2/forecast?lat={latitude}&lon={longitude}&lang={language}&limit={days}&extra=true";
            HttpResponseMessage response = null;
            using (HttpRequestMessage request = new HttpRequestMessage())
            {
                request.RequestUri = new Uri(url);
                request.Method = HttpMethod.Get;
                response = _client.SendAsync(request).Result;
            }

            if (response.IsSuccessStatusCode == false) throw new Exception($"API call was unsuccesfull. Status code: {response.StatusCode}.");

            string json = await response.Content.ReadAsStringAsync();
            _weatherForecast = JsonConvert.DeserializeObject<WeatherForecastModel>(json);
            return _weatherForecast;
        }

        public async Task<WeatherForecastModel> UpdateWeatherForecast(LocationModel location, bool forceUpdate = false)
        {
            var cachedWeatherForecast = _weatherForecasts.Find(x => x.geo_object.locality.name.ToLower() == location.City.ToLower());
            if (forceUpdate == false && cachedWeatherForecast != null) return cachedWeatherForecast;
            else if(forceUpdate == true && cachedWeatherForecast != null)
            {
                cachedWeatherForecast = await GetForecastAsync(location.Lat, location.Lng);
                return cachedWeatherForecast;
            }
            var updatedForecast = await GetForecastAsync(location.Lat, location.Lng);
            _weatherForecasts.Add(updatedForecast);
            return updatedForecast;
        }

        public PrecipitationMessageModel CheckPrecipitation(Forecast forecast)
        {
            if (forecast == null) throw new ArgumentNullException(nameof(forecast));

            Hour precipitationStart = forecast.hours.Skip(6).FirstOrDefault(x => x.prec_type != 0);
            if (precipitationStart == null) return null;

            List<Hour> precipitationHours = new List<Hour>();
            int startIndex = Convert.ToInt32(precipitationStart.hour);
            int count = 0;
            for (int i = startIndex; i < forecast.hours.Length; i++)
            {
                if (forecast.hours[i].prec_type == precipitationStart.prec_type)
                {
                    count++;
                    continue;
                }
                if (count >= 3)
                {
                    precipitationHours.AddRange(forecast.hours.ToList().GetRange(startIndex, count + 1));
                    break;
                }
                precipitationStart = forecast.hours.Skip(startIndex + 1 + count).FirstOrDefault(x => x.prec_type != 0);
                if (precipitationStart == null) return null;
                count = 0;
                startIndex = Convert.ToInt32(precipitationStart.hour);
            }

            if (precipitationHours.Count == 0) return null;

            if (WeatherForecastFormatter.precipitationTypes.TryGetValue(precipitationStart.prec_type, out string precipitationType) == false) throw new Exception($"Unregistered type of precipitation ({precipitationStart.prec_type}).");
            string extraAdvice = "";
            if (precipitationStart.prec_type == 1) extraAdvice = " Не забудьте взять с собой зонт.";

            DateTime startOfPrecipitationDateTime = DateTimeOffset.FromUnixTimeSeconds(precipitationStart.hour_ts).LocalDateTime;
            DateTime endOfPrecipitationDateTime = DateTimeOffset.FromUnixTimeSeconds(precipitationHours.Last().hour_ts).LocalDateTime;

            string day = "";
            switch (startOfPrecipitationDateTime.Day - DateTime.Now.Day)
            {
                case 0:
                    day = "Сегодня";
                    break;
                case 1:
                    day = "Завтра";
                    break;
                default:
                    day = startOfPrecipitationDateTime.ToString("В dddd, dd MMMM");
                    break;
            }

            return new PrecipitationMessageModel(header: $"{day} будет {precipitationType}.", body: $"Он начнётся в {startOfPrecipitationDateTime:HH:mm} и закончится к {endOfPrecipitationDateTime:HH:mm}.{extraAdvice}", startOfPrecipitationDateTime, endOfPrecipitationDateTime);
        }

        public MessageModel CheckTemperature(Forecast firstDay, Forecast nextDay)
        {
            if (firstDay == null) throw new ArgumentNullException(nameof(firstDay));
            if (nextDay == null) throw new ArgumentNullException(nameof(nextDay));

            int tomorrowTemperature = nextDay.parts.day_short.temp;
            int difference = firstDay.parts.day_short.temp - tomorrowTemperature;

            if (Math.Abs(difference) > 10)
            {
                if (difference < 0 && tomorrowTemperature > 25)
                {
                    return new MessageModel(header: "Завтра будет жарко!", body: $"Температура днём поднимется до {tomorrowTemperature}°C. Будьте осторожны, пейте больше воды и старайтесь не стоять долго под солнцем.");
                }
                else if (difference > 0)
                {
                    return new MessageModel(header: "Завтра резкое похолодание!", body: $"Температура днём опустится до {tomorrowTemperature}°C. Оденьтесь потеплее.");
                }
            }

            return null;
        }
    }
}
