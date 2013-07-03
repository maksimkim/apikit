namespace ApiKit.Routing.Cfg
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    public class RouteHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            //todo: add xsd validation
            //todo: add configuration inheritance from resource to resource or resource to method

            XDocument doc;

            using (var reader = new XmlNodeReader(section))
                doc = XDocument.Load(reader);

            var routes =
                from route in doc.Root.Element("resources").Elements("resource")
                select new Resource
                {
                    Type = route.Attribute("type").Value,

                    Uri = route.Attribute("uri").Value,

                    Methods = (
                        from method in route.Element("methods").Elements("method")
                        select new Method
                        {
                            Verb = method.Attribute("verb").Value,

                            //Aspects =
                            //    method.Attribute("aspects") != null
                            //    ? method.Attribute("aspects").Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()).ToArray()
                            //    : Enumerable.Empty<string>(),

                            //Authorization =
                            //    method.Element("authorization") != null
                            //    ? (
                            //        from rule in method.Element("authorization").Elements("rule")
                            //        select new AuthorizationRule
                            //        {
                            //            Type = rule.Attribute("type").Value,
                            //            Value = rule.Attribute("value").Value
                            //        }
                            //    ).ToArray()
                            //    : Enumerable.Empty<AuthorizationRule>()
                        }
                    ).ToArray(),

                    Constraints =
                        route.Element("constraints") != null
                        ? route.Element("constraints").Elements("add").ToDictionary(
                            e => e.Attribute("name").Value, 
                            e => e.Attribute("value").Value
                        ) : null,

                    Defaults =
                        route.Element("defaults") != null
                        ? route.Element("defaults").Elements("add").ToDictionary(
                            e => e.Attribute("name").Value, 
                            e => e.Attribute("value").Value
                        ) : null,
                };

            return routes.ToArray();
        }
    }
}