using System;
using System.Collections.Generic;
using System.Linq;
using WeatherForecastBackend.Models;

namespace WeatherForecastBackend
{
    public static class WeatherForecastFormatter
    {
        static public Dictionary<string, string> conditions = new Dictionary<string, string>()
        {
            ["clear"] = "Ясно",
            ["partly-cloudy"] = "Малооблачно",
            ["cloudy"] = "Облачно с прояснениями",
            ["overcast"] = "Пасмурно",
            ["drizzle"] = "Морось",
            ["light-rain"] = "Небольшой дождь",
            ["rain"] = "Дождь",
            ["moderate-rain"] = "Умеренно сильный дождь",
            ["heavy-rain"] = "Сильный дождь",
            ["continuous-heavy-rain"] = "Длительный сильный дождь",
            ["showers"] = "Ливень",
            ["wet-snow"] = "Дождь со снегом",
            ["light-snow"] = "Небольшой снег",
            ["snow"] = "Снег",
            ["snow-showers"] = "Снегопад",
            ["hail"] = "Град",
            ["thunderstorm"] = "Гроза",
            ["thunderstorm-with-rain"] = "Дождь с грозой",
            ["thunderstorm-with-hail"] = "Гроза с градом"

        };

        static public Dictionary<string, string> windDirections = new Dictionary<string, string>()
        {
            ["nw"] = "северо-западный",
            ["n"] = "северный",
            ["ne"] = "северо-восточный",
            ["e"] = "восточный",
            ["se"] = "юго-восточный",
            ["s"] = "южный",
            ["sw"] = "юго-западный",
            ["w"] = "западный",
            ["c"] = "штиль"
        };

        static public Dictionary<float, string> cloudnesses = new Dictionary<float, string>()
        {
            [0] = "Ясно",
            [0.25f] = "Малооблачно",
            [0.5f] = "Облачно с прояснениями",
            [0.75f] = "Облачно с прояснениями",
            [1] = "Пасмурно"
        };

        static public Dictionary<int, string> precipitationTypes = new Dictionary<int, string>()
        {
            [0] = "без осадков",
            [1] = "дождь",
            [2] = "дождь со снегом",
            [3] = "снег",
            [4] = "град"
        };

        public static string FormatPressure(float pressureInMM)
        {
            return $"Давление {pressureInMM} мм";
        }

        public static string FormatTemperature(int temp, bool inCelsius = true)
        {
            return inCelsius ? $"{temp}°C" : $"{temp * 1.8f + 32}°F";
        }

        public static string FormatHumidity(int humidity)
        {
            return $"Влажность {humidity}%";
        }

        public static string FormatUVIndex(int uv_index)
        {
            return $"УФ индекс {uv_index}";
        }

        public static string FormatFeelsLike(int feels_like, bool inCelsius = true)
        {
            return $"По ощущениям {feels_like}" + (inCelsius ? "°C" : "°F");
        }

        public static string GetIconUri(string iconCode)
        {
            return $@"https://yastatic.net/weather/i/icons/funky/dark/{iconCode}.svg";
        }

        public static string FormatFullAddress(string countryName, string provinceName, string localityName, string district)
        {
            List<string> validNames = new List<string>();
            if (countryName != null) validNames.Add(countryName);
            if (provinceName != null) validNames.Add(provinceName);
            if (localityName != null) validNames.Add(localityName);
            if (district != null) validNames.Add(district);
            validNames = validNames.Distinct().ToList();
            return string.Join(", ", validNames);
        }

        public static string GetWindSummary(float wind_speed, string wind_dir)
        {
            string windDirectionRU = wind_dir;
            windDirections.TryGetValue(wind_dir, out windDirectionRU);
            return $"Ветер {wind_speed} м/с, {windDirectionRU}";
        }
    }

}
