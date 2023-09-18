using Microsoft.AspNetCore.Mvc;

namespace BAMS.Controllers
{
    public class ErrorController : Controller
    {
        // GET
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

    }
}