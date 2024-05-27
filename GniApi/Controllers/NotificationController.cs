using GniApi.Dtos.RequestDto;
using GniApi.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GniApi.Controllers
{
    [Route("api/v1/notification")]
    //[ServiceFilter(typeof(InternalHeaderCheckActionFilter))]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;

        private readonly IConfiguration configuration;

        public NotificationController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }

        [HttpPost("send")]

        //Security needed
        public async Task<IActionResult> SendNotification([FromBody] NotificationActionRequestDto requestDto)
        {
            
            // Validate authorization
            var auth = HttpContext.Request.Headers["SecretKey"].ToString();
            if (string.IsNullOrEmpty(auth) || auth != configuration.GetValue<string>("SecretKey"))
            {
                return Unauthorized();
            }

            // Create HttpClient
            var client = httpClientFactory.CreateClient("mobileClient");

            // Construct Request URI
            var builder = new UriBuilder(client.BaseAddress + $"/customer/{requestDto.Pin}/loan-request/{requestDto.RequestId}/notification");

            // Construct Request Body
            var requestBody = new
            {
                actionKey = requestDto.Action
            };

            // Serialize Request Data
            string jsonString = JsonSerializer.Serialize(requestBody);

            // Log request data (optional)
            await Console.Out.WriteLineAsync(jsonString);

            // Create Request Content
            byte[] contentBytes = Encoding.UTF8.GetBytes(jsonString);
            var content = new ByteArrayContent(contentBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Send Request
            HttpResponseMessage response = await client.PutAsync(builder.Uri, content);

            // Handle Response
            string result = await response.Content.ReadAsStringAsync();
            // Log response data (optional)
            await Console.Out.WriteLineAsync(result);

            // Return Appropriate Response
            if (response.IsSuccessStatusCode)
            {
                // Success
                await Console.Out.WriteLineAsync("Notification sent successfully");
                return Ok();
            }
            else
            {
                // Failure
                await Console.Out.WriteLineAsync($"Failed to send notification. Status code: {response.StatusCode}");
                return StatusCode((int)response.StatusCode);
            }
        }

    }
}
