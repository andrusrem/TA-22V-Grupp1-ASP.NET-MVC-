using Microsoft.AspNetCore.Mvc;
using static Newtonsoft.Json.Linq.JObject;


namespace KooliProjekt.Services
{
    public class TunniTeenuseKlass
    {

        public class WeatherData
        {
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
            public decimal Generationtime_ms { get; set; }
            public WeatherUnits Current_units { get; set; }
            public WeatherX Current { get; set; }
            public WeatherUnits Hourly_units { get; set; }
            public WeatherHourly Hourly { get; set; }


        }

        public class WeatherUnits
        {
            public string Time { get; set; }
            public string Interval { get; set; }
            public string Temperature_2m { get; set; }
            public string Wind_speed_10m { get; set; }
            public string Relative_humidity_2m { get; set; }

        }

        public class WeatherX
        {
            public DateTime Time { get; set; }
            public int Interval { get; set; }
            public decimal Temperature_2m { get; set; }
            public decimal Wind_speed_10m { get; set; }
        }

        public class WeatherHourly
        {
            public DateTime[] Time { get; set; }
            public decimal[] Temperature_2m { get; set; }
            public int[] Relative_humidity_2m { get; set; }
            public decimal[] Wind_speed_10m { get; set; }

        }

        public class WeatherService
        {
            private readonly IWeatherService _weatherService;

            public WeatherService(IWeatherService service)
            {
                _weatherService = service;
            }
            public async Task<WeatherData> GetData()
            {
                var data = await _weatherService.GetData();
                return data;
            }
        }

        public interface IWeatherService
        {
            Task<WeatherData> GetData();
        }

        public class OpenMeteoClient : IWeatherService
        {
            public async Task<WeatherData> GetData()
            {

                using var client = new HttpClient();

                return await client.GetFromJsonAsync<WeatherData>("https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&current=temperature_2m,wind_speed_10m&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m");


            }
        }
        

    }
}