namespace ApiKit.Routing
{
    using Contracts.Auth;

    public class Method : IAuthorized, ISecured
    {
        public string Verb { get; set; }
        
        public IAuthorizationRule Rule { get; private set; }
        
        public bool? Private { get; private set; }
        
        public bool? Secure { get; private set; }
    }
}