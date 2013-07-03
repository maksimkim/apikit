namespace ApiKit.Contracts
{
    using System.Net;
    using System.Net.Http;

    public static class HttpRequestMessageExtensions
    {
         public static HttpResponseMessage CreateResponse(this HttpRequestMessage request, HttpStatusCode statusCode, string description)
         {
             var resp = request.CreateResponse(statusCode);
             resp.ReasonPhrase = description;
             return resp;
         }
    }
}