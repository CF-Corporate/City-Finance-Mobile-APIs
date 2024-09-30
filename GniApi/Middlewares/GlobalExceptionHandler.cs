using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace GniApi.CustomExtensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void CustomExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        int statusCode = (int)HttpStatusCode.InternalServerError;
                        string message = "Internal server error";
                        string traceId = context.TraceIdentifier;

                        // Check the type of exception and adjust the status code and message accordingly
                        switch (contextFeature.Error)
                        {
                            case BadHttpRequestException badRequest:
                                statusCode = (int)HttpStatusCode.BadRequest;
                                message = "Invalid JSON payload.";
                                break;

                            case ValidationException validationException:
                                statusCode = (int)HttpStatusCode.BadRequest;
                                message = "One or more validation errors occurred.";
                                break;

                            case JsonException jsonException:
                                statusCode = (int)HttpStatusCode.BadRequest;
                                message = "Malformed JSON detected.";
                                break;

                                // Handle more specific exceptions as needed
                        }

                        // Log the exception (optional)
                        // logger.LogError(contextFeature.Error, "An error occurred: {Message}", contextFeature.Error.Message);

                        // Set the response status code
                        context.Response.StatusCode = statusCode;

                        // Create a structured error response
                        var errorResponse = new
                        {
                            type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                            title = message,
                            status = statusCode,
                            traceId = traceId,
                            errors = new
                            {
                                model = new[] { contextFeature.Error.Message } // Capture the exception message
                            }
                        };

                        // Return the JSON response
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
                    }
                });
            });
        }
    }
}
