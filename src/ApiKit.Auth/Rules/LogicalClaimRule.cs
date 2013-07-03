namespace ApiKit.Auth.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using Contracts.Auth;

    public class LogicalClaimRule : IAuthorizationRule
    {
        private readonly IEnumerable<IAuthorizationRule> _children;

        private readonly Func<bool, bool, bool> _functor;

        public LogicalClaimRule(IEnumerable<IAuthorizationRule> children, Func<bool, bool, bool> functor)
        {
            if (children == null)
                throw new ArgumentException("children");

            if (_functor == null)
                throw new ArgumentException("functor");

            _children = children;

            _functor = functor;
        }

        public bool Apply(HttpRequestMessage request, ClaimsPrincipal caller)
        {
            return _children.Aggregate(true, (_, rule) => _functor(_, rule.Apply(request, caller)));
        }
    }
}