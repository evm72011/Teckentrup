using Microsoft.AspNetCore.Http;

namespace Gateway.Models
{
    public static class HttpResponseExtension
    {
        /// <summary>
        /// Adds information from OperationResult to the Header in HttpResponse
        /// </summary>
        /// <param name="response"></param>
        /// <param name="result"></param>
        public static void WrapHeaderWithResult(this HttpResponse response, OperationResult result)
        {
            response.Headers.Add("success", result.Success.ToString());
            response.Headers.Add("message", result.Message);
        }
    }
}
