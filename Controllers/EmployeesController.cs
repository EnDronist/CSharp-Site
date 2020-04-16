using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using DNS_Site.API;
using DNS_Site.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DNS_Site.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private const int _pageSize = 5;
        private readonly ILogger<EmployeesController> _logger;
        public EmployeesController(ILogger<EmployeesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Employee.GetResponse>> Get([FromQuery] int page = 0)
        {
            // Database query
            // There is "LocalSqlServer", web.config is not working
            //var connectionString = ConfigurationManager.ConnectionStrings;
            var result = new List<Employee.GetResponse>();
            using (var connection = new SqlConnection(ConnectionStrings.Default))
            {
                // Opening connection
                connection.Open();
                // Configuring command
                var command = new SqlCommand(@"
                    select [Id], [Name], [Surname], [Patronymic], [Department],
                        [Position], [Supervisor], [JobStartDate] from [Employees]
	                    order by [Id] offset @OffsetValue rows fetch next @OffsetNext rows only;
                ", connection);
                // Filling parameters
                command.Parameters.AddWithValue("@OffsetValue", page * _pageSize);
                command.Parameters.AddWithValue("@OffsetNext", _pageSize);
                // Executing
                var reader = await command.ExecuteReaderAsync();
                // Reading query result
                while (reader.Read())
                {
                    result.Add(new Employee.GetResponse(reader));
                }
                reader.Close();
                connection.Close();
            }
            return result;
        }

        [HttpPut]
        public async Task<Employee.PutResponse> Put([FromBody] Employee.PutRequest body)
        {
            var returnValue = new Employee.PutResponse();
            // Validation
            var errors = await Employee.PutRequest.Validation(body);
            if (errors.Count() != 0)
            {
                returnValue.ValidationErrors = errors;
                return returnValue;
            }
            // Database query
            using (var connection = new SqlConnection(ConnectionStrings.Default))
            {
                // Opening connection
                connection.Open();
                // Configuring command
                var command = new SqlCommand(@"
                    begin transaction;

                    insert into [Employees](
                    	[Name], [Surname], [Patronymic], [Department], [Position], [Supervisor]
                    )
                    values(
                        @Name, @Surname, @Patronymic, @Department, @Position, @Supervisor
                    );

                    select top (1) [Id] from [Employees]
                        order by [Id] desc;

                    commit;
                ", connection);
                // Filling parameters
                command.Parameters.AddWithValue("@Name", body.Name);
                command.Parameters.AddWithValue("@Surname", body.Surname);
                command.Parameters.AddWithValue("@Patronymic", body.Patronymic);
                command.Parameters.AddWithValue("@Department", body.Department);
                command.Parameters.AddWithValue("@Position", body.Position);
                command.Parameters.AddWithValue("@Supervisor", body.Supervisor);
                // Executing
                var reader = await command.ExecuteReaderAsync();
                _logger.LogInformation("Put: Inserted in database.");
                // Reading query result
                if (reader.Read())
                    returnValue.Result = reader.GetInt32(0);
                reader.Close();
                // Closing connection
                connection.Close();
            }
            return returnValue;
        }

        [HttpDelete]
        public async Task Delete([FromQuery]int? id)
        {
            // Check parameters
            if (id == null)
            {
                Response.StatusCode = 400; /* Bad Request */
                return;
            }
            // Database query
            using (var connection = new SqlConnection(ConnectionStrings.Default))
            {
                // Opening connection
                connection.Open();
                // Checking for row existence
                {
                    // Configuring command
                    var command = new SqlCommand(@"
                        select count(*) from [Employees]
                            where [Id] = @Id;
                    ", connection);
                    // Filling parameters
                    command.Parameters.AddWithValue("@Id", id);
                    // Executing
                    var reader = await command.ExecuteReaderAsync();
                    // Reading query result
                    if (reader.Read() && reader.GetInt32(0) == 0)
                    {
                        Response.StatusCode = 404; /* Not Found */
                        return;
                    }
                    reader.Close();
                }
                // Deleting row
                {
                    // Configuring transaction
                    var transaction = connection.BeginTransaction();
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    // Copying row into "EmployeesDelete" table (with losing "Supervisor" column)
                    command.CommandText = @"
                        insert into [EmployeesDeleted](
                        	[Name], [Surname], [Patronymic], [Department],
                            [Position], [Supervisor], [JobStartDate]
                        )
                        select [Name], [Surname], [Patronymic], [Department],
                            [Position], [Supervisor], [JobStartDate]
                        	from [Employees] where [Id] = @Id;
                    ";
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                    // Deleting row
                    command.CommandText = @"
                        delete from [Employees] where [Id] = @Id;
                    ";
                    await command.ExecuteNonQueryAsync();
                    // Finishing transaction
                    transaction.Commit();
                }
                // Finishing
                Response.StatusCode = 200; /* OK */
                return;
            }
        }
    }
}
