using Microsoft.AspNetCore.Http;

namespace Gateway.Models
{
    public static class HttpResponseExtension
    {
        public static void WrapHeaderWithResult(this HttpResponse response, OperationResult result)
        {
            response.Headers.Add("success", result.Success.ToString());
            response.Headers.Add("message", result.Message);
        }
    }
}
