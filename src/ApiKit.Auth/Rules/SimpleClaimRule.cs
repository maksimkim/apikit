namespace ApiKit.Auth.Rules
{
    using System.Net.Http;
    using System.Security.Claims;
    using Contracts.Auth;

    public class SimpleClaimRule : IAuthorizationRule
    {
        private readonly string _claimType;

        private readonly string _claimValue;

        public SimpleClaimRule(string claimType, string claimValue)
        {
            _claimType = claimType;

            _claimValue = claimValue;
        }

        public bool Apply(HttpRequestMessage request, ClaimsPrincipal caller)
        {
            return caller.HasClaim(_claimType, _claimValue);
        }
    }
}