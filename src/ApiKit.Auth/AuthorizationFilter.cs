namespace ApiKit.Auth
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Threading;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using Common.Logging;
    using Contracts.Auth;

    public class AuthorizationFilter : AuthorizeAttribute
    {
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private readonly IAuthorizationRuleProvider _ruleProvider;

        public AuthorizationFilter(IAuthorizationRuleProvider ruleProvider)
        {
            _ruleProvider = ruleProvider;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var request = actionContext.Request;

            var uri = request.RequestUri;

            Logger.Trace(m => m("Begin request {0} authorization", uri));

            var rawPrincipal = Thread.CurrentPrincipal;

            if (rawPrincipal == null)
            {
                Logger.Trace(m => m("Unauthenticated request. Authorization completed."));
                return true;
            }

            var principal = rawPrincipal as ClaimsPrincipal;

            if (principal == null)
                throw new InvalidOperationException(string.Format("Invalid principal type {0}. Authorization supported only for ClaimsPrincipal", rawPrincipal.GetType()));

            var routeData = request.GetRouteData();

            var method = request.Method;

            var routeTemplate = routeData.Route.RouteTemplate;

            Logger.Trace(m => m("Obtaining authorization rule for route {0} /{1}", method, routeTemplate));

            var rule = _ruleProvider.Get(routeTemplate, method);

            if (rule == null)
            {
                Logger.Trace(m => m("Authorization rule wasn't found for  {0} /{1}. Authorization completed.", method, routeTemplate));
                return true;
            }

            Logger.Trace(m => m("Applying authorization rule for {0} /{1}.", method, uri));

            var success = rule.Apply(request, principal);

            if (success)
                Logger.Trace(m => m("Access granted for user {0} to {1} /{2}", principal.Identity.Name, method, uri));
            else
                Logger.Trace(m => m("Access denied for user {0} to {1} /{2}", principal.Identity.Name, method, uri));

            return success;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.ControllerContext.Request.CreateResponse(HttpStatusCode.Forbidden);
        }
    }
}