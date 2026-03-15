using GestionFerias_CTPINVU.Data;
using GestionFerias_CTPINVU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string correo, string clave)
    {
        // Compute SHA256 of the input password to match seed
        string hashClave;
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(clave);
            var hashBytes = sha256.ComputeHash(bytes);
            hashClave = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == correo
                                   && u.PasswordHash == hashClave
                                   && u.Estado == "Activo");

        if (usuario != null)
        {
            HttpContext.Session.SetString("Usuario", usuario.Correo);
            HttpContext.Session.SetString("UsuarioId", usuario.UsuarioId.ToString());
            return RedirectToAction("Inicio", "Inicio");
        }

        TempData["ErrorLogin"] = "Correo o contraseña incorrectos.";
        TempData["CorreoLogin"] = correo;
        return View();
    }

    [HttpGet]
    public IActionResult OlvideContrasena()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }
}