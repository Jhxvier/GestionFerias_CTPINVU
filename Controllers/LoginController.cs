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

    [HttpGet]
    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registro(string documento, string nombres, string apellidos, string correo, string clave)
    {
        if (string.IsNullOrWhiteSpace(documento) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(clave))
        {
            TempData["ErrorRegistro"] = "Por favor, complete todos los campos obligatorios.";
            return View();
        }

        // Verificar documento
        bool personaExiste = await _context.Personas.AnyAsync(p => p.Documento == documento);
        if (personaExiste)
        {
            TempData["ErrorRegistro"] = "El número de documento ya está registrado.";
            return View();
        }

        // Verificar correo
        bool correoExiste = await _context.Usuarios.AnyAsync(u => u.Correo == correo);
        if (correoExiste)
        {
            TempData["ErrorRegistro"] = "El correo electrónico ya está registrado.";
            return View();
        }

        // Encriptar clave
        string hashClave;
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(clave);
            var hashBytes = sha256.ComputeHash(bytes);
            hashClave = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        // Crear Persona
        var nuevaPersona = new Persona
        {
            Documento = documento,
            Nombres = nombres,
            Apellidos = apellidos,
            FechaCreacion = DateTime.Now
        };
        _context.Personas.Add(nuevaPersona);
        await _context.SaveChangesAsync(); 

        // Crear Usuario asociado
        var nuevoUsuario = new Usuario
        {
            PersonaId = nuevaPersona.PersonaId,
            Correo = correo,
            PasswordHash = hashClave,
            Estado = "Activo",
            FechaCreacion = DateTime.Now
        };
        _context.Usuarios.Add(nuevoUsuario);
        await _context.SaveChangesAsync();

        // Opcional: Podrías asignarle el rol genérico al usuario registrado
        // var rol = await _context.Roles.FirstOrDefaultAsync(r => r.NombreRol == "Estudiante");
        // if (rol != null) 
        // {
        //     _context.UsuarioRoles.Add(new UsuarioRole { UsuarioId = nuevoUsuario.UsuarioId, RolId = rol.RolId });
        //     await _context.SaveChangesAsync();
        // }

        TempData["ErrorLogin"] = null; // Clear if coming from previous errors
        return RedirectToAction("Login", "Account");
    }
}