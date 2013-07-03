namespace ApiKit.Core.HyperMedia
{
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Collections.Generic;
    using Contracts;

    public class EndpointsFormattingComponent : IHostableComponent
    {
        private readonly MediaTypeFormatterCollection _storage;

        private readonly IEnumerable<MediaTypeFormatter> _formatters;

        public EndpointsFormattingComponent(MediaTypeFormatterCollection storage, IEnumerable<MediaTypeFormatter> formatters)
        {
            _storage = storage;

            _formatters = formatters.ToArray();
        }

        public void Start()
        {
            //insted of removing default formatters 
            var addingMediaTypes = _formatters.SelectMany(f => f.SupportedMediaTypes).ToList();

            foreach (var formatter in _storage.ToList())
            {
                var deletingMediaTypes = formatter.SupportedMediaTypes.Intersect(addingMediaTypes).ToList();

                if (!deletingMediaTypes.Any())
                    continue;

                if (deletingMediaTypes.SequenceEqual(formatter.SupportedMediaTypes))
                    _storage.Remove(formatter);
                else
                    foreach (var mediaType in deletingMediaTypes)
                        formatter.SupportedMediaTypes.Remove(mediaType);
            }

            foreach (var formatter in _formatters)
                _storage.Add(formatter);

            _storage.Remove(_storage.OfType<XmlMediaTypeFormatter>().FirstOrDefault());
        }

        public void Stop() { }
    }
}
