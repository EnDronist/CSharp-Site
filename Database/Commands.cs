using DNS_Site.API;
using DNS_Site.Controllers;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DNS_Site.Database
{
    public static class Queries
    {
        public static async Task<(IEnumerable<Employee.GetResponse>, int?/* Error code */)> GetEmployees(int page)
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
                command.Parameters.AddWithValue("@OffsetValue", page * EmployeesController.pageSize);
                command.Parameters.AddWithValue("@OffsetNext", EmployeesController.pageSize);
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
            return (result, null);
        }

        public static async Task<(Employee.PutResponse, int?/* Error code */)> PutEmployee(Employee.PutRequest data)
        {
            var returnValue = new Employee.PutResponse();
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
                command.Parameters.AddWithValue("@Name", data.Name);
                command.Parameters.AddWithValue("@Surname", data.Surname);
                command.Parameters.AddWithValue("@Patronymic", data.Patronymic);
                command.Parameters.AddWithValue("@Department", data.Department);
                command.Parameters.AddWithValue("@Position", data.Position);
                command.Parameters.AddWithValue("@Supervisor", data.Supervisor);
                // Executing
                var reader = await command.ExecuteReaderAsync();
                // Reading query result
                if (reader.Read())
                    returnValue.Result = reader.GetInt32(0);
                reader.Close();
                // Closing connection
                connection.Close();
            }
            return (returnValue, null);
        }

        public static async Task<int?/* Error code */> DeleteEmployee(int id)
        {
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
                        return 404; /* Not Found */
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
            }
            return null;
        }
    }
}
