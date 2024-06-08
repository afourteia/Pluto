using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;

namespace Pluto
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Welcome to the Home page!");
        }

        [HttpGet("about")]
        public IActionResult About()
        {
            var paramValue = HttpContext.Request.QueryString.Value;
            var test = QueryHelpers.ParseNullableQuery(paramValue);
            if (test != null && test.TryGetValue("param", out var value))
            {
                var testObject = JsonSerializer.Deserialize<Dictionary<string, object>>(value);
                return Ok($"About page. Query parameter value: {value}");
            }
            // paramValue = "test=1&key=123&value=abc";
            var paramObject = JsonSerializer.Deserialize<Dictionary<string, object>>(paramValue);
            if (paramObject != null && paramObject.ContainsKey("key"))
            {
                return Ok($"About page. Query parameter value exist");
                var paramValue1 = paramObject["key"].ToString();
                return Ok($"About page. Query parameter value: {paramValue1}");
            }
            return Ok($"About page. No Query parameter value found.");
        }

    }
}