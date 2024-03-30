using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Laptop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LaptopController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            var query = Request.QueryString.HasValue ? Request.QueryString.Value : string.Empty;

            return $"GET - Hello World - QueryString: {query}";
        }

        [HttpPost]
        public async Task<string> PostAsync()
        {
            var query = Request.QueryString.HasValue ? Request.QueryString.Value : string.Empty;

            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var textFromBody = await reader.ReadToEndAsync();

            return $"POST - Body: {textFromBody} - QueryString: {query}";
        }
    }
}
