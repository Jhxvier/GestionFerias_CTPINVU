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
            // Generate Random Password
            string newPass = Guid.NewGuid().ToString().Substring(0, 8); // 8 char alphanumeric

            // Hash the password
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(newPass);
                var hashBytes = sha256.ComputeHash(bytes);
                usuario.PasswordHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }

            // Save to DB
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            // Send Email
            string subject = "Recuperación de Contraseña - Sistema de Ferias INVU";
            string names = usuario.Persona?.Nombres ?? "Usuario";
            string bodyHtml = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 8px;'>
                    <h2 style='color: #BF1D1A; text-align: center;'>Sistema de Ferias INVU</h2>
                    <p>Hola <strong>{names}</strong>,</p>
                    <p>Has solicitado reestablecer tu contraseña. Tu nueva contraseña temporal es:</p>
                    <div style='background-color: #f4f4f4; padding: 15px; text-align: center; border-radius: 5px; font-size: 20px; font-weight: bold; letter-spacing: 2px; margin: 20px 0;'>
                        {newPass}
                    </div>
                    <p>Por favor, usa esta contraseña para ingresar al sistema y te recomendamos cambiarla enseguida editando tu Perfil.</p>
                    <hr style='border: 0; border-top: 1px solid #eee; margin: 30px 0;'/>
                    <p style='font-size: 12px; color: #999; text-align: center;'>Este es un mensaje automático. Por favor no respondas a este correo.</p>
                </div>";

            await _emailService.SendEmailAsync(usuario.Correo, subject, bodyHtml);
        }

        // Always show the same generic success message to prevent user-enumeration attacks
        TempData["MensajeRecuperacion"] = $"Si la cuenta existe, se ha enviado un correo con instrucciones a {correo}. Por favor verifique su bandeja de entrada o carpeta de Spam.";
        
        return View();
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }
}