using Microsoft.AspNetCore.Mvc;

namespace GestionFerias_CTPINVU.Controllers
{
    public class ErrorController : Controller
    {
        // Errores no manejados (500)
        [Route("Error/Index")]
        public IActionResult Index()
        {
            return View("Error500");
        }

        // Errores por código de estado (403, 404, etc.)
        [Route("Error/Status/{statusCode}")]
        public IActionResult Status(int statusCode)
        {
            if (statusCode == 403)
                return View("Error403");

            if (statusCode == 404)
                return View("Error404");

            return View("ErrorGeneral", statusCode);
        }
    }
}
