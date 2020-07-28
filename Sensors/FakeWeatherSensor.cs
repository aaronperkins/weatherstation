using System;

namespace WeatherStation.Sensors
{
    public class FakeWeatherSensor : IWeatherSensor
    {
        public WeatherReading GetReading()
        {
            var rng = new Random();
            var result = new WeatherReading()
            {
                TemperatureC = (float)rng.Next(0, 40),
                Pressure = (float)rng.Next(700, 1300)
            };

            return result;
        }
    }
}
