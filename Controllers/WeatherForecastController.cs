using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace DNS_Site.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();
            // There is "LocalSqlServer", web.config is not working
            //var connectionString = ConfigurationManager.ConnectionStrings;
            var connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=DNS-Site;Integrated Security=True;Pooling=true";
            var result = new List<string>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT TOP (5) [Name] FROM [Employees];
                ", connection);
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    _logger.LogInformation($"{reader.GetString(0)}");
                    result.Add(reader.GetString(0));
                }
                connection.Close();
            }

            var range = Enumerable.Range(0, 5);

            return range.Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index + 1),
                TemperatureC = rng.Next(-20, 55),
                Name = Request.Query.FirstOrDefault(p => p.Key == "page").Value /*result.ElementAtOrDefault(index)*/,
            })
            .ToArray();
        }
    }
}