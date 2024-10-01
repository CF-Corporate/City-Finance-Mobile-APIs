using GniApi.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace GniApi.CustomExtensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void CustomExceptionHadler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    string message = "Internal error";

                    int statuscode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    var exception = contextFeature?.Error;

                    if (exception is EntryNotFoundException)
                    {
                        statuscode = (int)HttpStatusCode.BadRequest;
                        message = exception.Message;
                    }
                   
                    context.Response.StatusCode = statuscode;

                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(
                            new { statusCode = statuscode, errorText = contextFeature.Error.Message }
                        ));
                    }
                });
            });
        }
    }
}
