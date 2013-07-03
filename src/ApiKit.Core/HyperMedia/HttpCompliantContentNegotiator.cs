namespace ApiKit.Core.HyperMedia
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Collections.Generic;

    public class HttpCompliantContentNegotiator : DefaultContentNegotiator
    {
        private class UnsupportedAcceptTypeMediaTypeFormatter : MediaTypeFormatter
        {
            private class TypeMapping : MediaTypeMapping
            {
                public TypeMapping() : base("text/plain") { }

                public override double TryMatchMediaType(HttpRequestMessage request)
                {
                    return double.Epsilon;
                }
            }

            public UnsupportedAcceptTypeMediaTypeFormatter()
            {
                MediaTypeMappings.Add(new TypeMapping());
            }

            public override bool CanReadType(Type type) { return false; }

            public override bool CanWriteType(Type type) { return true; }
        }

        private static readonly UnsupportedAcceptTypeMediaTypeFormatter UnsupportedAcceptTypeFormatter = new UnsupportedAcceptTypeMediaTypeFormatter();

        public override ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            if (!IsResponseFormatDefinedByClient(request))
                return base.Negotiate(type, request, formatters);

            var result = base.Negotiate(type, request, formatters.Concat(new[] { UnsupportedAcceptTypeFormatter }));

            return result.Formatter == UnsupportedAcceptTypeFormatter ? null : result;
        }

        private static bool IsResponseFormatDefinedByClient(HttpRequestMessage request)
        {
            return request.Headers.Accept != null && request.Headers.Accept.Count > 0;
        }
    }
}
