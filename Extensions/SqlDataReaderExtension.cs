using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DNS_Site.Extensions
{
    public static class SqlDataReaderExtension
    {
        public static int? GetInt32OrNull(this SqlDataReader reader, int columnIndex)
        {
            if (!reader.IsDBNull(columnIndex))
            {
                return reader.GetInt32(columnIndex);
            }
            return null;
        }
        public static string GetStringOrNull(this SqlDataReader reader, int columnIndex)
        {
            if (!reader.IsDBNull(columnIndex))
            {
                return reader.GetString(columnIndex);
            }
            return null;
        }
        public static bool? GetBooleanOrNull(this SqlDataReader reader, int columnIndex)
        {
            if (!reader.IsDBNull(columnIndex))
            {
                return reader.GetBoolean(columnIndex);
            }
            return null;
        }
    }
}
