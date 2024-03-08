using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class WeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<WeatherData> GetDataAsync()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&current=temperature_2m,wind_speed_10m&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m");
            response.EnsureSuccessStatusCode(); // Throw exception if not successful
            string content = await response.Content.ReadAsStringAsync();
            
            // Deserialize JSON response to WeatherData object
            WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(content);
            
            return weatherData;
        }
        catch (Exception ex)
        {
            // Handle exception
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }
}
