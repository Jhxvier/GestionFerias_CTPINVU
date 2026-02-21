using Microsoft.AspNetCore.Mvc;

namespace GestionFerias_CTPINVU.Controllers
{
    public class InicioController : Controller
    {
        public IActionResult Inicio()
        {
            return View();
        }
    }
}
