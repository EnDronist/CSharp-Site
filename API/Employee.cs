using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DNS_Site.Database;

using DNS_Site.Extensions;

/**
 * Classes that are located here are definition of data
 * exchanged between the client and server
 */

namespace DNS_Site.API
{
    public static class Employee
    {
        public class GetRequest
        {
            // From Query (GET method)
            // int page = 0
        }
        public class GetResponse /* Array of */
        {
            // Fields
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Patronymic { get; set; }
            public string Department { get; set; }
            public string Position { get; set; }
            public int? Supervisor { get; set; }
            public DateTime JobStartDate { get; set; }

            // For custom initialization
            public GetResponse() { }

            // Full initialization
            public GetResponse(SqlDataReader reader)
            {
                Id = reader.GetInt32(0);
                Name = reader.GetString(1);
                Surname = reader.GetString(2);
                Patronymic = reader.GetStringOrNull(3);
                Department = reader.GetString(4);
                Position = reader.GetString(5);
                Supervisor = reader.GetInt32OrNull(6);
                JobStartDate = reader.GetDateTime(7);
            }
        }
        public class PutRequest
        {
            // Fields
            // Auto-increasing primary key
            //public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Patronymic { get; set; }
            public string Department { get; set; }
            public string Position { get; set; }
            public int? Supervisor { get; set; }
            // Auto-generating timestamp
            //public DateTime JobStartDate { get; set; }

            public static async Task<Dictionary<string, string>> Validation(PutRequest obj)
            {
                // Preparing
                var errors = new Dictionary<string, string>();
                string fieldName;
                // Name
                fieldName = nameof(obj.Name).ToCamelCase();
                if (obj.Name == null || Regex.Matches(obj.Name, "^[a-zA-Z0-9-_]{1,50}$").Count == 0)
                    errors.Add(fieldName, $"Incorrect \"{fieldName}\" field value");
                // Surname
                fieldName = nameof(obj.Surname).ToCamelCase();
                if (obj.Surname == null || Regex.Matches(obj.Surname, "^[a-zA-Z0-9-_]{1,50}$").Count == 0)
                    errors.Add(fieldName, $"Incorrect \"{fieldName}\" field value");
                // Patronymic
                fieldName = nameof(obj.Patronymic).ToCamelCase();
                if (obj.Patronymic != null && Regex.Matches(obj.Patronymic, "^[a-zA-Z0-9-_]{1,50}$").Count == 0)
                    errors.Add(fieldName, $"Incorrect \"{fieldName}\" field value");
                // Department
                fieldName = nameof(obj.Department).ToCamelCase();
                if (obj.Department == null || Regex.Matches(obj.Department, "^[a-zA-Z0-9-_]{1,50}$").Count == 0)
                    errors.Add(fieldName, $"Incorrect \"{fieldName}\" field value");
                // Position
                fieldName = nameof(obj.Position).ToCamelCase();
                if (obj.Position == null || Regex.Matches(obj.Position, "^[a-zA-Z0-9-_]{1,50}$").Count == 0)
                    errors.Add(fieldName, $"Incorrect \"{fieldName}\" field value");
                // Supervisor
                fieldName = nameof(obj.Supervisor).ToCamelCase();
                using (var connection = new SqlConnection(ConnectionStrings.Default))
                {
                    // Opening connection
                    connection.Open();
                    // Configuring command
                    var command = new SqlCommand(@"
                        select count(*) from [Employees]
                            where [Id] = @Supervisor;
                    ", connection);
                    // Filling parameters
                    command.Parameters.AddWithValue("@Supervisor", obj.Supervisor);
                    // Executing
                    var reader = await command.ExecuteReaderAsync();
                    // Reading query result
                    if (reader.Read() && reader.GetInt32(0) == 0)
                        errors.Add(fieldName, $"Specified \"{fieldName}\" not exists");
                    reader.Close();
                    // Closing connection
                    connection.Close();
                }
                // Result
                return errors;
            }
        }
        public class PutResponse
        {
            public int Result { get; set; } = -1;
            public Dictionary<string, string> ValidationErrors { get; set; }
        }

        public class DeleteRequest
        {
            // From Query
            // int? id - required
        }

        //public class DeleteResponse = void
    }
}