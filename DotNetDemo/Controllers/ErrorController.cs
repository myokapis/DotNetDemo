using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DotNetDemo.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index([FromServices] IHostEnvironment hostEnvironment)
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
            // TODO: augment the log with Trace Id
            Log.Error(feature.Error, "Unknown Error");

            return Problem("Unknown Error");
        }
    }
}
