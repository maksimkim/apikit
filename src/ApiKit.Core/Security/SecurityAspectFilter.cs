namespace ApiKit.Core.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    public class SecurityAspectFilter : IAuthorizationFilter
    {
        private readonly IDictionary<Tuple<string, HttpMethod>, IEnumerable<IAuthorizationFilter>> _filterMap;

        public bool AllowMultiple
        {
            get { return false; }
        }

        public SecurityAspectFilter(IDictionary<Tuple<string, HttpMethod>, IEnumerable<IAuthorizationFilter>> filterMap)
        {
            _filterMap = filterMap;
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            if (_filterMap.Count == 0)
                return continuation();
            
            var request = actionContext.Request;

            var routeData = request.GetRouteData();

            var key = Tuple.Create(routeData.Route.RouteTemplate, request.Method);

            IEnumerable<IAuthorizationFilter> filters;

            if (!_filterMap.TryGetValue(key, out filters) || !filters.Any())
                return continuation();

            var queue = new Queue<IAuthorizationFilter>(filters);

            return Apply(queue.Dequeue(), queue, actionContext, cancellationToken, continuation);
        }

        private Task<HttpResponseMessage> Apply(
            IAuthorizationFilter head,
            Queue<IAuthorizationFilter> tail,
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation
        )
        {
            return head.ExecuteAuthorizationFilterAsync(
                actionContext,
                cancellationToken,
                () => tail.Count == 0 ? continuation() : Apply(tail.Dequeue(), tail, actionContext, cancellationToken, continuation)
            );
        }
    }
}