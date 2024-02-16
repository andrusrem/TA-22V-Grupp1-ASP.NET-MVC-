using System;

public class WeatherX
{
    public DateTime Time {get; set;}
    public int interval {get; set;}
    public decimal temperature_2m {get; set;}
    public decimal wind_speed_10m {get; set;}
}

public class WeatherUnits
{
    public string Time {get; set;}
    public string interval {get; set;}
    public string temperature_2m { get; set; }
    public string relative_humidity_2m { get; set; }
    public string wind_speed_10m { get; set; }
}

public class WeatherData
{
    public decimal latitude {get; set;}
    public decimal longitude {get; set;}

    public WeatherUnits Current_units {get; set;}
    public WeatherX Current {get; set;}
    public WeatherUnits Hourly_units {get; set;}

}
