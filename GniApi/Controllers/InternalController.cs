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
    [Route("api/internal")]
    //[ServiceFilter(typeof(InternalHeaderCheckActionFilter))]
    [ApiController]
    [ApiKeyAuthorizationFilter("Internal")]
    public class InternalController : ControllerBase
    {

        private readonly IHttpClientFactory httpClientFactory;

        public InternalController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> InitializeAction([FromBody] dynamic jsonData)
        {
            var client = httpClientFactory.CreateClient("gniClient");

            var builder = new UriBuilder(client.BaseAddress + "/controller");

            string jsonString = JsonSerializer.Serialize(jsonData);

            await Console.Out.WriteLineAsync(   jsonString);

            //byte[] contentBytes = Encoding.UTF8.GetBytes(jsonString);

            //var content = new ByteArrayContent(contentBytes);

            //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //HttpResponseMessage response = await client.PostAsync(builder.Uri, content);


            //string result = await response.Content.ReadAsStringAsync();



            await Console.Out.WriteLineAsync( "Mock Send Request" );

            return Ok();


        }
    }
}
