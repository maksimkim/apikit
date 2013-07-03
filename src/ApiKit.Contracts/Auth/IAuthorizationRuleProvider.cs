namespace ApiKit.Contracts.Auth
{
    using System.Net.Http;

    public interface IAuthorizationRuleProvider
    {
        IAuthorizationRule Get(string uriPattern, HttpMethod method);
    }
}