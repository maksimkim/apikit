using System;
using System.Collections.Generic;

namespace ApiKit.Ninject
{
    using Contracts;
    using Contracts.Dependency;

    public class NinjectResolver : IComponentResolver
    {
        public IEnumerable<IHostableComponent> Resolve()
        {
            throw new NotImplementedException();
        }
    }
}
