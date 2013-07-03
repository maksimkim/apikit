namespace ApiKit.Contracts.Dependency
{
    using System.Collections.Generic;

    public interface IComponentResolver
    {
        IEnumerable<IHostableComponent> Resolve();
    }
}