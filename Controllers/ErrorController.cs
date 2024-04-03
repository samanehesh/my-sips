using Microsoft.AspNetCore.Mvc;

namespace Sips.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Error/StatusCodePage")]
        public IActionResult StatusCodePage(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound", statusCode);
            } 
            return View("Error", statusCode);
        }
    }
}
