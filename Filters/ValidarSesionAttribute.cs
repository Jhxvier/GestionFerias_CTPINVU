using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace GestionFerias_CTPINVU.Filters
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerName = context.RouteData.Values["controller"]?.ToString();

            // Excluir el controlador de Cuenta y Errores del bloqueo para prevenir redirecciones infinitas
            if (controllerName != "Account" && controllerName != "Error")
            {
                var usuarioId = context.HttpContext.Session.GetString("UsuarioId");
                
                if (string.IsNullOrEmpty(usuarioId))
                {
                    // Redirigir al inicio de sesión si no hay un identificador válido
                    context.Result = new RedirectToActionResult("Login", "Account", null);
                    return;
                }
            }

            // Inhabilitar globalmente el guardado temporal (cache) del explorador (evita ver pantallas protegidas con el boton Atrás)
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            context.HttpContext.Response.Headers["Expires"] = "0";

            base.OnActionExecuting(context);
        }
    }
}
