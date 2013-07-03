using System;
using System.Collections.Generic;

namespace ApiKit.Autofac
{
    using Contracts;
    using Contracts.Dependency;

    public class AutofacResolver : IComponentResolver
    {
        public IEnumerable<IHostableComponent> Resolve()
        {
            throw new NotImplementedException();
        }
    }
}
