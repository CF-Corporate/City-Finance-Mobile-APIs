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
        public async Task<IActionResult> SendNotification([FromBody] dynamic jsonData)
        {


            var auth = HttpContext.Request.Headers["SecretKey"].ToString();

            if (string.IsNullOrEmpty(auth) || auth != configuration.GetValue<string>("SecretKey"))
            {
                return Unauthorized();
            }


            //var client = httpClientFactory.CreateClient("gniClient");

            //var builder = new UriBuilder(client.BaseAddress + "/controller");

            //string jsonString = JsonSerializer.Serialize(jsonData);

            //await Console.Out.WriteLineAsync(jsonString);

            //byte[] contentBytes = Encoding.UTF8.GetBytes(jsonString);

            //var content = new ByteArrayContent(contentBytes);

            //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //HttpResponseMessage response = await client.PostAsync(builder.Uri, content);


            //string result = await response.Content.ReadAsStringAsync();



            await Console.Out.WriteLineAsync("Mock Send Request");

            return Ok();


        }
    }
}
