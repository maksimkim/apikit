namespace ApiKit.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Routing;
    using Common.Logging;
    using Contracts;

    public class RouteComponent : IHostableComponent
    {
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private readonly RouteCollection _routes;

        private readonly Resource[] _resources;

        public RouteComponent(RouteCollection routes, Resource[] resources)
        {
            _routes = routes;
            _resources = resources;
        }

        public void Start()
        {
            if (_resources.Length == 0)
            {
                Logger.Error(m => m("No routes founded"));
                return;
            }

            Logger.Info(m => m("Registering routes"));

            Logger.Info(m => m("{0} routes founded", _resources.Length));

            var error = false;

            foreach (var cfg in _resources.OrderByDescending(e => e.Uri.Count(c => c == '/')))
            {
                var routeKey = cfg.Uri;

                Logger.Debug(m => m("Registering route {0}", routeKey));

                try
                {
                    var typeName = cfg.Type;

                    if (string.IsNullOrEmpty(typeName))
                        throw new InvalidOperationException("Controller type not set for route " + routeKey);

                    var controllerType = Type.GetType(typeName, true);

                    IDictionary<string, object> defaults = null;

                    if (cfg.Defaults != null)
                        foreach (var elm in cfg.Defaults)
                            defaults.Add(elm.Key, elm.Value);

                    var constraints = new Dictionary<string, object>();

                    if (cfg.Constraints != null)
                        foreach (var elm in cfg.Constraints)
                            constraints.Add(elm.Key, elm.Value);

                    var route = _routes.MapHttpRoute(cfg.Uri, cfg.Uri, defaults, constraints);

                    route.DataTokens = new RouteValueDictionary();

                    route.Defaults["controller"] = controllerType.Name.Replace("Controller", string.Empty);

                    route.DataTokens["Namespaces"] = new[] { controllerType.Namespace };

                    Logger.Debug(m => m("Route {0} successfully registered.", routeKey));
                }
                catch (Exception ex)
                {
                    Logger.Error(m => m("Route {0} registration failed", routeKey), ex);

                    //todo: add setting {apikit.throwOnRouteError}
                    //throw;

                    if (!error)
                        error = true;
                }
            }

            if (!error)
                Logger.Info(m => m("Route registration successfully finished"));
            else
                Logger.Error(m => m("Route registration finished with errors"));
        }

        public void Stop()
        {
            
        }
    }
}