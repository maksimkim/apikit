namespace ApiKit.Contracts.Auth
{
    using System.Net.Http;
    using System.Security.Principal;
    using System.Threading.Tasks;

    public interface IAuthenticationScheme
    {
        Task<IPrincipal> Authenticate(string data, HttpRequestMessage request);
    }
}