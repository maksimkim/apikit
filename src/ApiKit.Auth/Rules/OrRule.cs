namespace ApiKit.Auth.Rules
{
    using System;
    using System.Collections.Generic;
    using Contracts.Auth;

    public class OrRule : LogicalClaimRule
    {
        public OrRule(IEnumerable<IAuthorizationRule> children, Func<bool, bool, bool> functor) 
            : base(children, (a, b) => a || b)
        {
        }
    }
}