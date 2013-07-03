namespace ApiKit.Core.Validation
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Contracts;
    using Contracts.Validation;

    public class ModelValidationFilter : ActionFilterAttribute
    {
        private readonly IKitSettings _settings;

        private static readonly Regex PrefixTrimmer = new Regex(@"^[^\.]+\.", RegexOptions.Compiled);

        public ModelValidationFilter(IKitSettings settings)
        {
            _settings = settings;
            
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            foreach (var exclude in _settings.ValidationExcludes)
                actionContext.ModelState.Remove(exclude);

            if (actionContext.ModelState.IsValid)
                return;

            var firstError = actionContext.ModelState
                .Where(s => s.Value.Errors.Count > 0)
                .Select(s =>
                {
                    var error = s.Value.Errors.First();

                    return new
                    {
                        Name = PrefixTrimmer.Replace(s.Key, string.Empty).ToLower(),
                        Messages =
                            !string.IsNullOrEmpty(error.ErrorMessage)
                            ? error.ErrorMessage
                            : (error.Exception != null ? error.Exception.Message : string.Empty)
                    };
                }).FirstOrDefault();


            if (firstError == null)
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest);
            else
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.BadRequest, 
                    new ValidationErrorResponse
                    {
                        Source = firstError.Name,
                        Details = firstError.Messages
                    }
                );
        }
    }
}