using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.Helpers
{
    public static class ExtensionMethodManager
    {
        public static string TruncateAndTrim(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) 
                return value;

            return value.Length <= maxLength ? value.Trim() : value.Substring(0, maxLength).Trim();

        }
    }
}
