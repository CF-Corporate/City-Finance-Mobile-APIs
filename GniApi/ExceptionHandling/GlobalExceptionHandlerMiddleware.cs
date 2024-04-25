using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace GniApi.ExceptionHandling;
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Log the exception or perform any necessary actions
        Console.WriteLine($"An unhandled exception occurred: {exception.Message}");

        // Set the response to 400 Bad Request with a custom message
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";

        var response = new { message = "System xətası baş verdi. Yenidən yoxlayın", code = HttpStatusCode.BadRequest };
        var jsonResponse = JsonConvert.SerializeObject(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}
