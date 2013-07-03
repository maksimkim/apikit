namespace ApiKit.Core.HyperMedia
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http;

    public class UnsupportedContentTypeMediaTypeFormatter : MediaTypeFormatter
    {
        private static readonly HttpResponseException Ex = new HttpResponseException(new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType));
        
        public override bool CanReadType(Type type)
        {
            throw Ex;
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }
    }
}