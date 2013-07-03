namespace ApiKit.Core.Filters
{
    using System.Collections.Generic;
    using System.Web.Http.Filters;
    using Contracts;

    public class GlobalFilterComponent : IHostableComponent
    {
        private readonly HttpFilterCollection _storage;

        private readonly IEnumerable<IFilter> _filters;

        public GlobalFilterComponent(HttpFilterCollection storage, IEnumerable<IFilter> filters)
        {
            _storage = storage;

            _filters = filters;
        }

        public void Start()
        {
            foreach (var globalFilter in _filters)
                _storage.Add(globalFilter);
        }

        public void Stop()
        {
        }
    }
}