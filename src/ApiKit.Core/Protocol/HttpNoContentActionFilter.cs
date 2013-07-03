namespace ApiKit.Core.Protocol
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;
    using System.Web.Http.Controllers;
    using System.Threading;
    using System.Threading.Tasks;

    public class HttpNoContentActionFilter : IActionFilter
    {
        public bool AllowMultiple { get { return true; } }

        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var returnType = actionContext.ActionDescriptor.ReturnType;

            if (returnType != typeof(void) && returnType != typeof(Task) && returnType != null)
                return await continuation().ConfigureAwait(false);

            var response = await continuation().ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.OK && response.Content == null)
                response.StatusCode = HttpStatusCode.NoContent;

            return response;

        }
    }
}
