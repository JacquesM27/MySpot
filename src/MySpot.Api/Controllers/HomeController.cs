using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySpot.Infrastructure;

namespace MySpot.Api.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly AppOptions _appOptions;

        public HomeController(IOptions<AppOptions> options)
        {
            _appOptions = options.Value;
        }

        public ActionResult<string> Get() => _appOptions.Name;
    }
}
