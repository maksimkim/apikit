namespace ApiKit.Contracts.Auth
{
    using System.Net.Http;
    using System.Security.Claims;

    public interface IAuthorizationRule
    {
        //todo: Seems that the whole request is redundunt data for authorization process. RouteData is just enough
        bool Apply(HttpRequestMessage request, ClaimsPrincipal caller);
    }
}