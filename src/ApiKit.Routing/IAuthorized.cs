namespace ApiKit.Routing
{
    using Contracts.Auth;

    public interface IAuthorized
    {
        IAuthorizationRule Rule { get; }
    }
}