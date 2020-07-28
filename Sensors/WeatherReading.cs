using System;

namespace WeatherStation.Sensors
{
    public class WeatherReading 
    {
        public WeatherReading()
        {
            Date = Date = DateTime.UtcNow;
        }

        public DateTime Date { get; set; }

        public float TemperatureC { get; set; }

        public float TemperatureF => 32.0f + (TemperatureC / 0.5556f);

        public float Pressure { get; set; }
    }
}
