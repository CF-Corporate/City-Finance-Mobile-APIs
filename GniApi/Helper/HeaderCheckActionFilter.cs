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

            var authorization2 = context.HttpContext.Request.Cookies["ApiKey"];


            if (AuthenticationHeaderValue.TryParse(authorization, out var value))
            {
                if(value.ToString() != configuration.GetValue<string>("ApiKey"))
                {
                    context.Result= new UnauthorizedResult();
                }
                context.HttpContext.Response.Cookies.Append("ApiKey",value.ToString());
            }
             if (AuthenticationHeaderValue.TryParse(authorization, out var value2))
            {
                if (value2.ToString() != configuration.GetValue<string>("ApiKey"))
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();

            }
        }
    }
}
