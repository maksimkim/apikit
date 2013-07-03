namespace ApiKit.Core.Security
{
    using System;
    using System.Net;
    using System.Web.Http.Filters;
    using Contracts;

    public class RequireHttpsFilter : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var request = actionContext.Request;

            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
                actionContext.Response = request.CreateResponse((HttpStatusCode)426, "Upgrade Required");
        }
    }
}