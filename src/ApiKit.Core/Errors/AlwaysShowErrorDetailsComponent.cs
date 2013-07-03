namespace ApiKit.Core.Errors
{
    using System.Web.Http;
    using Contracts;

    public class AlwaysShowErrorDetailsComponent : IHostableComponent
    {
        private readonly IKitSettings _settings;

        private readonly HttpConfiguration _cfg;

        public AlwaysShowErrorDetailsComponent(IKitSettings settings, HttpConfiguration cfg)
        {
            _settings = settings;
            _cfg = cfg;
        }

        public void Start()
        {
            _cfg.IncludeErrorDetailPolicy = _settings.DetailedErrors ? IncludeErrorDetailPolicy.Always : _cfg.IncludeErrorDetailPolicy;
        }

        public void Stop()
        {
            
        }
    }
}