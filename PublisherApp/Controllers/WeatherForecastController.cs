using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenericPubSubLibrary;
using GenericPubSubLibrary.Interfaces;
using GenericPubSubLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PublisherApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController(IRabbitPublisher publisherService)
        {
            this.publisherService = publisherService;
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IRabbitPublisher publisherService;

        [HttpGet]
        public IActionResult Get()
        {

            var payload = new Payload
            {
                Body = new WeatherForecast()
                {
                    Date = DateTime.Now.AddDays(-1),
                    Summary = Summaries[new Random().Next(0, 10)],
                    TemperatureC = new Random().Next(1, 40)
                },

                CorrelationId = Guid.NewGuid().ToString(),
            };

            publisherService.Publish(payload, new MessagingConfiguration("WeatherForecastExchange"));

            return Ok();
        }
    }
}
