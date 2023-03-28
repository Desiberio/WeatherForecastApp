using System;
using System.Collections.Generic;
using static WeatherForecastBackend.Models.Forecast;
using System.Linq;

namespace WeatherForecastBackend.Models
{
    public class WeatherForecastModel
    {
        public int now { get; set; }
        public DateTime now_dt { get; set; }
        public Geo_object geo_object { get; set; }
        public Fact fact { get; set; }
        public Forecast[] forecasts { get; set; }
    }

    public class Geo_object
    {
        public string FullAddress => WeatherForecastFormatter.FormatFullAddress(country?.name, province?.name, locality?.name, district?.name);
        public District district { get; set; }
        public Locality locality { get; set; }
        public Province province { get; set; }
        public Country country { get; set; }
    }

    public class District
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Locality
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Province
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Country
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Fact
    {
        public string windSummary => WeatherForecastFormatter.GetWindSummary(wind_speed, wind_dir);

        public string UVIndexFormatted => WeatherForecastFormatter.FormatUVIndex(uv_index);
        public string factCondiotionRU => WeatherForecastFormatter.conditions.TryGetValue(condition, out string conditionRU) ? conditionRU : condition;
        public string tempFormatted => WeatherForecastFormatter.FormatTemperature(temp);
        public string feelsLikeFormatted => WeatherForecastFormatter.FormatFeelsLike(feels_like);
        public string humidityFormatted => WeatherForecastFormatter.FormatHumidity(humidity);

        public string cloudnessRU => WeatherForecastFormatter.cloudnesses.TryGetValue(cloudness, out string cloudnessRU) ? cloudnessRU : "Ошибка при загрузке облачности.";
        public string pressureFormated => WeatherForecastFormatter.FormatPressure(pressure_mm);
        public int obs_time { get; set; }
        public int uptime { get; set; }
        public int temp { get; set; }
        public int feels_like { get; set; }
        public string icon { get; set; }
        public string condition { get; set; }
        public float cloudness { get; set; }
        public int prec_type { get; set; }
        public int prec_prob { get; set; }
        public float prec_strength { get; set; }
        public bool is_thunder { get; set; }
        public float wind_speed { get; set; }
        public string wind_dir { get; set; }
        public int pressure_mm { get; set; }
        public int pressure_pa { get; set; }
        public int humidity { get; set; }
        public string daytime { get; set; }
        public bool polar { get; set; }
        public string season { get; set; }
        public string source { get; set; }
        public float soil_moisture { get; set; }
        public int soil_temp { get; set; }
        public int uv_index { get; set; }
        public float wind_gust { get; set; }
    }

    public partial class Forecast
    {
        public string dayConditionIconURL => WeatherForecastFormatter.GetIconUri(parts.day.icon);
        public string dayCondiotionRU
        {
            get
            {
                return WeatherForecastFormatter.conditions.TryGetValue(parts.day.condition, out string conditionRU) ? conditionRU : parts.day.condition;
            }
        }
        public string dayTempAvg
        {
            get => WeatherForecastFormatter.FormatTemperature(parts.day.temp_avg);
        }

        public DateTime date { get; set; }
        public int date_ts { get; set; }
        public int week { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string rise_begin { get; set; }
        public string set_end { get; set; }
        public int moon_code { get; set; }
        public string moon_text { get; set; }
        public Parts parts { get; set; }
        public Hour[] hours { get; set; }
    }

    public class Parts
    {
        public Night_Short night_short { get; set; }
        public Night night { get; set; }
        public Evening evening { get; set; }
        public Day_Short day_short { get; set; }
        public Morning morning { get; set; }
        public Day day { get; set; }
    }

    public class Night_Short
    {
        public int temp { get; set; }
        public float wind_speed { get; set; }
        public float wind_gust { get; set; }
        public string wind_dir { get; set; }
        public int pressure_mm { get; set; }
        public int pressure_pa { get; set; }
        public int humidity { get; set; }
        public int soil_temp { get; set; }
        public float soil_moisture { get; set; }
        public float prec_mm { get; set; }
        public int prec_prob { get; set; }
        public int prec_period { get; set; }
        public float cloudness { get; set; }
        public int prec_type { get; set; }
        public float prec_strength { get; set; }
        public string icon { get; set; }
        public string condition { get; set; }
        public int uv_index { get; set; }
        public int feels_like { get; set; }
        public string daytime { get; set; }
        public bool polar { get; set; }
        public float fresh_snow_mm { get; set; }
    }

    public class Night
    {
        public int temp_min { get; set; }
        public int temp_avg { get; set; }
        public int temp_max { get; set; }
        public float wind_speed { get; set; }
        public float wind_gust { get; set; }
        public string wind_dir { get; set; }
        public int pressure_mm { get; set; }
        public int pressure_pa { get; set; }
        public int humidity { get; set; }
        public int soil_temp { get; set; }
        public float soil_moisture { get; set; }
        public float prec_mm { get; set; }
        public int prec_prob { get; set; }
        public int prec_period { get; set; }
        public float cloudness { get; set; }
        public int prec_type { get; set; }
        public float prec_strength { get; set; }
        public string icon { get; set; }
        public string condition { get; set; }
        public int uv_index { get; set; }
        public int feels_like { get; set; }
        public string daytime { get; set; }
        public bool polar { get; set; }
        public float fresh_snow_mm { get; set; }
    }

    public class Evening
    {
        public int temp_min { get; set; }
        public int temp_avg { get; set; }
        public int temp_max { get; set; }
        public float wind_speed { get; set; }
        public float wind_gust { get; set; }
        public string wind_dir { get; set; }
        public int pressure_mm { get; set; }
        public int pressure_pa { get; set; }
        public int humidity { get; set; }
        public int soil_temp { get; set; }
        public float soil_moisture { get; set; }
        public float prec_mm { get; set; }
        public int prec_prob { get; set; }
        public int prec_period { get; set; }
        public float cloudness { get; set; }
        public int prec_type { get; set; }
        public float prec_strength { get; set; }
        public string icon { get; set; }
        public string condition { get; set; }
        public int uv_index { get; set; }
        public int feels_like { get; set; }
        public string daytime { get; set; }
        public bool polar { get; set; }
        public float fresh_snow_mm { get; set; }
    }

    public class Day_Short
    {
        public int temp { get; set; }
        public int temp_min { get; set; }
        public float wind_speed { get; set; }
        public float wind_gust { get; set; }
        public string wind_dir { get; set; }
        public int pressure_mm { get; set; }
        public int pressure_pa { get; set; }
        public int humidity { get; set; }
        public int soil_temp { get; set; }
        public float soil_moisture { get; set; }
        public float prec_mm { get; set; }
        public int prec_prob { get; set; }
        public int prec_period { get; set; }
        public float cloudness { get; set; }
        public int prec_type { get; set; }
        public float prec_strength { get; set; }
        public string icon { get; set; }
        public string condition { get; set; }
        public int uv_index { get; set; }
        public int feels_like { get; set; }
        public string daytime { get; set; }
        public bool polar { get; set; }
        public float fresh_snow_mm { get; set; }
    }

    public class Morning
    {
        public int temp_min { get; set; }
        public int temp_avg { get; set; }
        public int temp_max { get; set; }
        public float wind_speed { get; set; }
        public float wind_gust { get; set; }
        public string wind_dir { get; set; }
        public int pressure_mm { get; set; }
        public int pressure_pa { get; set; }
        public int humidity { get; set; }
        public int soil_temp { get; set; }
        public float soil_moisture { get; set; }
        public float prec_mm { get; set; }
        public int prec_prob { get; set; }
        public int prec_period { get; set; }
        public float cloudness { get; set; }
        public int prec_type { get; set; }
        public float prec_strength { get; set; }
        public string icon { get; set; }
        public string condition { get; set; }
        public int uv_index { get; set; }
        public int feels_like { get; set; }
        public string daytime { get; set; }
        public bool polar { get; set; }
        public float fresh_snow_mm { get; set; }
    }

    public class Day
    {
        public int temp_min { get; set; }
        public int temp_avg { get; set; }
        public int temp_max { get; set; }
        public float wind_speed { get; set; }
        public float wind_gust { get; set; }
        public string wind_dir { get; set; }
        public int pressure_mm { get; set; }
        public int pressure_pa { get; set; }
        public int humidity { get; set; }
        public int soil_temp { get; set; }
        public float soil_moisture { get; set; }
        public float prec_mm { get; set; }
        public int prec_prob { get; set; }
        public int prec_period { get; set; }
        public float cloudness { get; set; }
        public int prec_type { get; set; }
        public float prec_strength { get; set; }
        public string icon { get; set; }
        public string condition { get; set; }
        public int uv_index { get; set; }
        public int feels_like { get; set; }
        public string daytime { get; set; }
        public bool polar { get; set; }
        public float fresh_snow_mm { get; set; }
    }

    public class Hour
    {
        public string hour { get; set; }
        public int hour_ts { get; set; }
        public int temp { get; set; }
        public int feels_like { get; set; }
        public string icon { get; set; }
        public string condition { get; set; }
        public float cloudness { get; set; }
        public int prec_type { get; set; }
        public float prec_strength { get; set; }
        public bool is_thunder { get; set; }
        public string wind_dir { get; set; }
        public float wind_speed { get; set; }
        public float wind_gust { get; set; }
        public int pressure_mm { get; set; }
        public int pressure_pa { get; set; }
        public int humidity { get; set; }
        public int uv_index { get; set; }
        public int soil_temp { get; set; }
        public float soil_moisture { get; set; }
        public float prec_mm { get; set; }
        public int prec_period { get; set; }
        public int prec_prob { get; set; }
    }

}
