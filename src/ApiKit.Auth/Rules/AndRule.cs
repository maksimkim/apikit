namespace ApiKit.Auth.Rules
{
    using System.Collections.Generic;
    using Contracts.Auth;

    public class AndRule : LogicalClaimRule
    {
        public AndRule(IEnumerable<IAuthorizationRule> children) 
            : base(children, (a,b) => a && b)
        {
        }
    }
}