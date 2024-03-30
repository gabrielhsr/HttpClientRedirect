using Microsoft.AspNetCore.Mvc;

namespace HttpClientRedirect.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string baseUrl = "Laptop";

        public ServerController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await GetInterceptor(string.Empty);
        }

        [HttpGet("{*path}")]
        public async Task<IActionResult> GetWildCard(string path)
        {
            return await GetInterceptor(path);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            return await PostInterceptor(string.Empty);
        }

        [HttpPost("{*path}")]
        public async Task<IActionResult> PostWildCard(string path)
        {
            return await PostInterceptor(path);
        }


        public async Task<IActionResult> GetInterceptor(string path)
        {
            using var client = httpClientFactory.CreateClient("NamedClient");

            var query = Request.QueryString.HasValue ? Request.QueryString.Value : string.Empty;
            var url = $"{baseUrl}/{path}{query}";

            try
            {
                var res = await client.GetAsync(url);

                if (res.IsSuccessStatusCode)
                {
                    var stream = await res.Content.ReadAsStringAsync();

                    return Ok(stream);
                }

                return BadRequest($"FAIL - Get ({url}) - {res.StatusCode}");
            }
            catch (Exception ex)
            {
                return BadRequest($"FAIL - Get ({url}) - {ex.Message}");
            }
        }

        public async Task<IActionResult> PostInterceptor(string path)
        {
            using var client = httpClientFactory.CreateClient("NamedClient");

            var query = Request.QueryString.HasValue ? Request.QueryString.Value : string.Empty;
            var url = $"{baseUrl}/{path}{query}";

            using var reader = new StreamReader(Request.Body);
            var bodyContent = await reader.ReadToEndAsync();

            try
            {
                var res = await client.PostAsync(url, new StringContent(bodyContent));

                if (res.IsSuccessStatusCode)
                {
                    var stream = await res.Content.ReadAsStringAsync();

                    return Ok(stream);
                }

                return BadRequest($"FAIL - Post ({url}) - {res.StatusCode}");
            }
            catch (Exception ex)
            {
                return BadRequest($"FAIL - Post ({url}) - {ex.Message}");
            }
        }
    }
}
