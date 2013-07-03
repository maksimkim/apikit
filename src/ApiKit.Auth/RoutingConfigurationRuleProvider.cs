namespace ApiKit.Auth
{
    using System.Net.Http;
    using Contracts.Auth;

    public class RoutingConfigurationRuleProvider : IAuthorizationRuleProvider
    {
        public IAuthorizationRule Get(string uriPattern, HttpMethod method)
        {
            throw new System.NotImplementedException();
        }
    }
}