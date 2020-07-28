namespace WeatherStation.Sensors
{
    public interface IWeatherSensor
    {
        WeatherReading GetReading();
    }
}
