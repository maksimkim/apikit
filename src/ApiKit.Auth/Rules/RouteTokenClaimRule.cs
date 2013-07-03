namespace ApiKit.Auth.Rules
{
    using System;
    using System.Net.Http;
    using System.Security.Claims;
    using Contracts.Auth;

    public class RouteTokenClaimRule : IAuthorizationRule
    {
        private readonly string _claimType;

        private readonly string _routeToken;

        public RouteTokenClaimRule(string claimType, string routeToken)
        {
            _claimType = claimType;

            _routeToken = routeToken;
        }

        public bool Apply(HttpRequestMessage request, ClaimsPrincipal caller)
        {
            var routeValues = request.GetRouteData().Values;

            if (!routeValues.ContainsKey(_routeToken))
                return false;

            var val = routeValues[_routeToken] as string;

            if (string.IsNullOrWhiteSpace(val))
                throw new InvalidOperationException("Only uri template tokens can be used for Authorization");

            return caller.HasClaim(_claimType, val);
        }
    }
}