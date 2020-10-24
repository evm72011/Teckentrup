using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Headers;
using System.Linq;

namespace Client
{
    public static class HttpResponseHeadersExtension
    {
        public static bool HasKey(this HttpResponseHeaders headers, string key)
        {
            return headers.Where(header => header.Key == key).Count() > 0;
        }

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

    }
}
