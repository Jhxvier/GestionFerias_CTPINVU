using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionFerias_CTPINVU.Data;
using GestionFerias_CTPINVU.Models;
using GestionFerias_CTPINVU.Services;

public class AccountController : Controller
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;

    public AccountController(AppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string correo, string clave)
    {
        //encriptar la clave usando SHA256
        string hashClave;
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(clave);
            var hashBytes = sha256.ComputeHash(bytes);
            hashClave = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        var usuario = await _context.Usuarios
            .Include(u => u.Estudiante)
            .Include(u => u.Tutore)
            .Include(u => u.Juece)
            .Include(u => u.UsuarioRoles).ThenInclude(ur => ur.Rol)
            .FirstOrDefaultAsync(u => u.Correo == correo
                                   && u.PasswordHash == hashClave);

        if (usuario != null)
        {
            // Verificar si el usuario está inactivo
            if (usuario.Estado != "Activo")
            {
                TempData["ErrorLogin"] = "Su cuenta se encuentra inactiva. Contacte al administrador del sistema para reactivarla.";
                TempData["CorreoLogin"] = correo;
                return View();
            }

            string rolNombre = "Usuario";
            if (usuario.UsuarioRoles.Any(ur => ur.RolId == 1)) rolNombre = "Administrador";
            else if (usuario.Estudiante != null) rolNombre = "Estudiante";
            else if (usuario.Tutore != null) rolNombre = "Tutor";
            else if (usuario.Juece != null) rolNombre = "Juez";
            else rolNombre = "Coordinador"; // Fallback para coordinadores huérfanos sin rol formal en tabla
            
            HttpContext.Session.SetString("Usuario", usuario.Correo);
            HttpContext.Session.SetString("UsuarioId", usuario.UsuarioId.ToString());
            HttpContext.Session.SetString("Rol", rolNombre);

            //actualizar el ultimo acceso
            usuario.UltimoAcceso = DateTime.Now;
            await _context.SaveChangesAsync();

            // If temp password → force change
            if (usuario.RequiereCambioClave)
            {
                HttpContext.Session.SetString("RequiereCambioClave", "true");
                return RedirectToAction("CambiarContrasena");
            }

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OlvideContrasena(string correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
        {
            TempData["MensajeRecuperacion"] = "Por favor ingrese un correo válido.";
            return View();
        }

        var usuario = await _context.Usuarios
            .Include(u => u.Persona)
            .FirstOrDefaultAsync(u => u.Correo == correo.Trim());

        if (usuario != null)
        {
            //generar una nueva contraseña temporal
            string newPass = Guid.NewGuid().ToString().Substring(0, 8); // 8 caracteres aleatorios

            // Encriptar la nueva contraseña usando SHA256
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(newPass);
                var hashBytes = sha256.ComputeHash(bytes);
                usuario.PasswordHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }

            // Mark as temporary password
            usuario.RequiereCambioClave = true;

            // Save to DB
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            // Send Email
            string subject = "Contraseña temporal - Sistema de Ferias INVU";
            string names = usuario.Persona?.Nombres ?? "Usuario";
            string bodyHtml = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 8px;'>
                    <h2 style='color: #BF1D1A; text-align: center;'>Sistema de Ferias INVU</h2>
                    <p>Hola <strong>{names}</strong>,</p>
                    <p>Has solicitado reestablecer tu contraseña. Se ha generado una <strong>contraseña temporal</strong>:</p>
                    <div style='background-color: #f4f4f4; padding: 15px; text-align: center; border-radius: 5px; font-size: 20px; font-weight: bold; letter-spacing: 2px; margin: 20px 0;'>
                        {newPass}
                    </div>
                    <div style='background: #fff3cd; border: 1px solid #ffc107; border-radius: 8px; padding: 12px; margin: 16px 0;'>
                        <strong>⚠ Importante:</strong> Esta contraseña es <strong>temporal</strong>. Al iniciar sesión se le solicitará crear una contraseña nueva de forma obligatoria. No podrá acceder al sistema hasta que la cambie.
                    </div>
                    <p>Si usted no solicitó este cambio, puede ignorar este mensaje.</p>
                    <hr style='border: 0; border-top: 1px solid #eee; margin: 30px 0;'/>
                    <p style='font-size: 12px; color: #999; text-align: center;'>Este es un mensaje automático. Por favor no respondas a este correo.</p>
                </div>";

            await _emailService.SendEmailAsync(usuario.Correo, subject, bodyHtml);
        }

        //permitir mostrar el mensaje aunque el correo no exista para evitar revelar información sobre cuentas registradas
        TempData["MensajeRecuperacion"] = $"Si la cuenta existe, se ha enviado un correo con instrucciones a {correo}. Por favor verifique su bandeja de entrada o carpeta de Spam.";
        
        return View();
    }

    // ─── Forced password change ──────────────────────────────

    [HttpGet]
    public IActionResult CambiarContrasena()
    {
        var requiere = HttpContext.Session.GetString("RequiereCambioClave");
        if (requiere != "true")
            return RedirectToAction("Inicio", "Inicio");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarContrasena(string nuevaClave, string confirmarClave)
    {
        var requiere = HttpContext.Session.GetString("RequiereCambioClave");
        if (requiere != "true")
            return RedirectToAction("Inicio", "Inicio");

        if (string.IsNullOrWhiteSpace(nuevaClave) || nuevaClave.Length < 6)
        {
            TempData["ErrorCambio"] = "La contraseña debe tener al menos 6 caracteres.";
            return View();
        }

        if (nuevaClave != confirmarClave)
        {
            TempData["ErrorCambio"] = "Las contraseñas no coinciden.";
            return View();
        }

        var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
        if (string.IsNullOrEmpty(usuarioIdStr))
            return RedirectToAction("Login");

        var usuario = await _context.Usuarios.FindAsync(long.Parse(usuarioIdStr));
        if (usuario == null) return RedirectToAction("Login");

        // Hash the new password
        using (var sha256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(nuevaClave);
            var hashBytes = sha256.ComputeHash(bytes);
            usuario.PasswordHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        usuario.RequiereCambioClave = false;
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();

        // Clear session flag
        HttpContext.Session.Remove("RequiereCambioClave");

        return RedirectToAction("Inicio", "Inicio");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }
}