namespace ApiKit.Core
{
    using System.Collections.Generic;
    using Common.Logging;
    using Contracts;
    using Contracts.Dependency;

    public class Kit
    {
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private readonly IComponentResolver _componentResolver;
        
        private IEnumerable<IHostableComponent> _components;

        public Kit(IComponentResolver componentResolver)
        {
            _componentResolver = componentResolver;
        }

        public void Start()
        {
            _components = _componentResolver.Resolve();
            
            foreach (var component in _components)
            {
                component.Start();
                Logger.Debug(m => m("Configured with component {0}", component.GetType()));
            }
        }

        public void Stop()
        {
            if (_components != null)
                foreach (var component in _components)
                    component.Stop();
        }
    }
}
