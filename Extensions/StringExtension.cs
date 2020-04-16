using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNS_Site.Extensions
{
    public static class StringExtension
    {
        public static string ToCamelCase(this string str)
        {
            if (str.Length > 0) return char.ToLowerInvariant(str[0]) + str.Substring(1);
            return str;
        }
        public static string ToTitleCase(this string str)
        {
            if (str.Length > 0) return char.ToUpperInvariant(str[0]) + str.Substring(1);
            return str;
        }
    }
}
