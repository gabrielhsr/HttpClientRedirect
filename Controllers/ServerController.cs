using Microsoft.AspNetCore.Mvc;

namespace HttpClientRedirect.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServerController : ControllerBase
    {
        private HttpClient laptopClient;

        public ServerController(IHttpClientFactory httpClientFactory)
        {
            laptopClient = httpClientFactory.CreateClient("ZKTeco");
        }

        [HttpGet]
        public async Task GetAsync()
        {
            var response = await laptopClient.GetAsync("Laptop");

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStringAsync();

                ////var data = await JsonSerializer.DeserializeAsync<string>(stream);
            }
        }
    }
}
