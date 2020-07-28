using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherStation.Sensors;

namespace WeatherStation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherSensor _sensor;

        public WeatherController(ILogger<WeatherController> logger, IWeatherSensor sensor)
        {
            _logger = logger;
            _sensor = sensor;
        }

        [HttpGet]
        public WeatherReading Get()
        {
            var result = _sensor.GetReading();
            return result;
        }
    }
}


