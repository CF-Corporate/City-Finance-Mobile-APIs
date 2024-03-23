using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http.Headers;

namespace GniApi.Helper
{
    public class HeaderCheckActionFilter : IActionFilter
    {
        private readonly IConfiguration configuration;

        public HeaderCheckActionFilter(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var authorization = context.HttpContext.Request.Headers["ApiKey"];

            if(AuthenticationHeaderValue.TryParse(authorization, out var value))
            {
                if(value.ToString() != configuration.GetValue<string>("ApiKey"))
                {
                    context.Result= new UnauthorizedResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();

            }
        }
    }
}
