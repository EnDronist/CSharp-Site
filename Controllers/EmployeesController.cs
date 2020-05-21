using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using DNS_Site.API;
using DNS_Site.Database;
using Microsoft.AspNetCore.Mvc.Infrastructure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DNS_Site.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        public const int pageSize = 5;
        private readonly ILogger<EmployeesController> _logger;
        private readonly IActionContextAccessor _accessor;
        public EmployeesController(
            ILogger<EmployeesController> logger,
            IActionContextAccessor accessor
        )
        {
            _logger = logger;
            _accessor = accessor;
        }

        [HttpGet]
        public async Task<IEnumerable<Employee.GetResponse>> Get([FromQuery] int page = 0)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await Queries.GetEmployees(page);
            if (result.Item2 != null)
            {
                Response.StatusCode = result.Item2.GetValueOrDefault();
                _logger.LogInformation($"Sended with code {Response.StatusCode} to {ipAddress}");
                return null;
            }
            // Finishing
            _logger.LogInformation($"Sended with code {Response.StatusCode} to {ipAddress}");
            return result.Item1;
        }

        [HttpPut]
        public async Task<Employee.PutResponse> Put([FromBody] Employee.PutRequest body)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var returnValue = new Employee.PutResponse();
            // Validation
            var errors = await Employee.PutRequest.Validation(body);
            if (errors.Count() != 0)
            {
                returnValue.ValidationErrors = errors;
                _logger.LogInformation($"Sended with code {Response.StatusCode} and validation errors to {ipAddress}");
                return returnValue;
            }
            // Database query
            var result = await Queries.PutEmployee(body);
            if (result.Item2 != null)
            {
                Response.StatusCode = result.Item2.GetValueOrDefault();
                _logger.LogInformation($"Sended with code {Response.StatusCode} to {ipAddress}");
                return null;
            }
            else
            {
                returnValue = result.Item1;
                _logger.LogInformation("Put: Inserted in database.");
            }
            // Finishing
            _logger.LogInformation($"Sended with code {Response.StatusCode} to {ipAddress}");
            return returnValue;
        }

        [HttpDelete]
        public async Task Delete([FromQuery]int? id)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            // Check parameters
            if (id == null)
            {
                Response.StatusCode = 400; /* Bad Request */
                _logger.LogInformation($"Sended with code {Response.StatusCode} to {ipAddress}");
                return;
            }
            // Database query
            var result = await Queries.DeleteEmployee(id.GetValueOrDefault(0));
            if (result != null)
            {
                Response.StatusCode = result.GetValueOrDefault();
                _logger.LogInformation($"Sended with code {Response.StatusCode} to {ipAddress}");
                return;
            }
            // Finishing
            Response.StatusCode = 200; /* OK */
            _logger.LogInformation($"Sended with code {Response.StatusCode} to {ipAddress}");
            return;
        }
    }
}