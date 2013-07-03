namespace ApiKit.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Common.Logging;
    using Contracts.Auth;

    public class AuthenticationFilter : IAuthorizationFilter
    {
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private readonly IDictionary<string, IAuthenticationScheme> _schemes;

        public AuthenticationFilter(IDictionary<string, IAuthenticationScheme> schemes)
        {
            _schemes = schemes;
        }

        public bool AllowMultiple
        {
            get { return false; }
        }

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var request = actionContext.Request;

            Logger.Trace(m => m("Begin request {0} authentication.", request.RequestUri));

            var auth = request.Headers.Authorization;

            if (auth == null)
            {
                Logger.Trace(m => m("Authorization header wasn't provided with request. Authentication completed."));
                return await continuation();
            }


            if (string.IsNullOrWhiteSpace(auth.Scheme) || string.IsNullOrWhiteSpace(auth.Parameter) || !_schemes.ContainsKey(auth.Scheme))
            {
                Logger.Trace(m => m("Authorization header has incorrect value {0}. Responsing with 401 (Unauthorized).", auth.Scheme));
                return request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var scheme = _schemes[auth.Scheme];

            Logger.Trace(m => m("Authenticating request with scheme {0}.", auth.Scheme));

            var user = await scheme.Authenticate(auth.Parameter, request).ConfigureAwait(continueOnCapturedContext: true);

            if (user == null)
            {
                Logger.Trace(m => m("Request wasn't authenticated with scheme {0}. Responsing with 401 (Unauthorized).", auth.Scheme));
                return request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            Logger.Trace(m => m("Request authorization through scheme {0} successeded with user {1}.", auth.Scheme, user.Identity.Name));

            var ctx = HttpContext.Current;

            Thread.CurrentPrincipal = ctx.User = user;

            Logger.Trace(m => m("Authentication completed"));

            return await continuation();
        }
    }
}