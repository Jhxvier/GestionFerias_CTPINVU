using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using GestionFerias_CTPINVU.ViewModels;
using System.Linq;

namespace GestionFerias_CTPINVU.Controllers
{
    public class InicioConfigData
    {
        public string Anio { get; set; } = "2026";
        public string ImagenPath { get; set; } = "~/images/Inicio.png";
    }

    public class InicioController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _configPath;

        public InicioController(IWebHostEnvironment env)
        {
            _env = env;
            _configPath = Path.Combine(_env.WebRootPath, "config_inicio.json");
        }

        private InicioConfigData GetConfig()
        {
            if (System.IO.File.Exists(_configPath))
            {
                var json = System.IO.File.ReadAllText(_configPath);
                return JsonSerializer.Deserialize<InicioConfigData>(json) ?? new InicioConfigData();
            }
            return new InicioConfigData();
        }

        private void SaveConfig(InicioConfigData data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_configPath, json);
        }

        public IActionResult Inicio()
        {
            var config = GetConfig();
            ViewData["Anio"] = config.Anio;
            ViewData["ImagenPath"] = config.ImagenPath + "?v=" + DateTime.Now.Ticks; // Prevent caching if overwritten

            var rol = HttpContext.Session.GetString("Rol") ?? "";
            ViewData["EsAdminOCoord"] = rol.Contains("Administrador", StringComparison.OrdinalIgnoreCase) ||
                                         rol.Contains("Coordinador", StringComparison.OrdinalIgnoreCase);

            return View();
        }

        [HttpGet]
        public IActionResult Configurar()
        {
            var rol = HttpContext.Session.GetString("Rol") ?? "";
            bool esAdminOCoord = rol.Contains("Administrador", StringComparison.OrdinalIgnoreCase) ||
                                 rol.Contains("Coordinador", StringComparison.OrdinalIgnoreCase);
            if (!esAdminOCoord) return StatusCode(403);

            var config = GetConfig();
            var vm = new ConfiguracionInicioViewModel
            {
                Anio = config.Anio,
                ImagenActual = config.ImagenPath
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Configurar(ConfiguracionInicioViewModel model)
        {
            var rol = HttpContext.Session.GetString("Rol") ?? "";
            bool esAdminOCoord = rol.Contains("Administrador", StringComparison.OrdinalIgnoreCase) ||
                                 rol.Contains("Coordinador", StringComparison.OrdinalIgnoreCase);
            if (!esAdminOCoord) return StatusCode(403);

            var config = GetConfig();
            model.ImagenActual = config.ImagenPath;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validar la imagen si fue subida
            if (model.ImagenNueva != null && model.ImagenNueva.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var ext = Path.GetExtension(model.ImagenNueva.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("ImagenNueva", "Formato de imagen inválido. Solo se permite Jpg, Png o Webp.");
                    return View(model);
                }

                if (model.ImagenNueva.Length > 5 * 1024 * 1024) // 5 MB max
                {
                    ModelState.AddModelError("ImagenNueva", "La imagen excede el tamaño máximo permitido de 5MB.");
                    return View(model);
                }

                // Guardar imagen
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = "hero_custom_" + Guid.NewGuid().ToString().Substring(0,8) + ext;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImagenNueva.CopyToAsync(fileStream);
                }

                // Si había una custom anterior, podríamos borrarla para ahorrar espacio
                if (config.ImagenPath.Contains("hero_custom_"))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, config.ImagenPath.Replace("~/", ""));
                    if (System.IO.File.Exists(oldPath))
                    {
                        try { System.IO.File.Delete(oldPath); } catch { }
                    }
                }

                config.ImagenPath = "~/images/" + uniqueFileName;
            }

            // Actualizar año
            config.Anio = model.Anio;

            SaveConfig(config);

            TempData["ToastExito"] = "La configuración de inicio se guardó correctamente.";
            return RedirectToAction("Inicio");
        }
    }
}
