using GestionFerias_CTPINVU.Models;
using GestionFerias_CTPINVU.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == correo
                                   && u.PasswordHash == clave
                                   && u.Estado == "Activo");

        if (usuario != null)
        {
            HttpContext.Session.SetString("Usuario", usuario.Correo);
            HttpContext.Session.SetString("UsuarioId", usuario.UsuarioId.ToString());

            return RedirectToAction("Index", "Eventos");
        }

        ViewBag.Error = "Correo o contraseña incorrectos.";
        return View();
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }
}