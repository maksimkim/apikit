namespace ApiKit.Routing
{
    using System.Collections.Generic;
    using Contracts.Auth;

    public class Resource : IAuthorized, ISecured
    {
        public string Uri { get; set; }

        public IEnumerable<Resource> Resources { get; set; }

        public IEnumerable<Method> Methods { get; set; }

        public string Type { get; set; }

        public IDictionary<string,string> Constraints { get; set; }

        public IDictionary<string, string> Defaults { get; set; }
        
        public IAuthorizationRule Rule { get; private set; }
        
        public bool? Private { get; private set; }
        
        public bool? Secure { get; private set; }
    }
}