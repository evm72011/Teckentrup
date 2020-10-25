using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Headers;
using System.Linq;

namespace Client
{
    public static class HttpResponseHeadersExtension
    {
        /// <summary>
        /// Checks that HttpResponseHeader contains the key
        /// </summary>
        public static bool HasKey(this HttpResponseHeaders headers, string key)
        {
            return headers.Where(header => header.Key == key).Count() > 0;
        }

        /// <summary>
        /// Checks that HttpResponseHeader contains all keys from the list
        /// </summary>
        public static bool HasKeys(this HttpResponseHeaders headers, params string[] keys)
        {
            foreach (var key in keys)
            {
                if (!headers.HasKey(key))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gives value of the key from the HttpResponseHeader and converts it to needed type
        /// </summary>
        public static T GetValue<T>(this HttpResponseHeaders headers, string key)
        {
            var valueStr = headers.GetValues(key).FirstOrDefault();
            return (T)Convert.ChangeType(valueStr, typeof(T));
        }
    }
}
