using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var validationErrors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new
                {
                    Field = e.Key,
                    Errors = e.Value.Errors.Select(error => error.ErrorMessage)
                });

            context.HttpContext.Response.StatusCode = 500;

            context.Result = new BadRequestObjectResult(new { statusCode = 1,errorText ="One or more parametrs isn`t found" });
        }
    }
}
