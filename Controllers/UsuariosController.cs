using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionFerias_CTPINVU.Data;
using GestionFerias_CTPINVU.Models;

using GestionFerias_CTPINVU.ViewModels;

namespace GestionFerias_CTPINVU.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? textoBuscar, string? filtroEstado)
        {
            var query = _context.Usuarios
                .Include(u => u.UsuarioCreacionNavigation)
                .Include(u => u.UsuarioModificacionNavigation)
                .Include(u => u.Estudiante)
                .Include(u => u.Juece)
                .Include(u => u.Tutore)
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                var lowerBuscar = textoBuscar.ToLower();
                query = query.Where(u => u.Correo.ToLower().Contains(lowerBuscar) ||
                                         (u.Persona != null && (u.Persona.Nombres.ToLower().Contains(lowerBuscar) || 
                                                                u.Persona.Apellidos.ToLower().Contains(lowerBuscar) ||
                                                                u.Persona.Documento.ToLower().Contains(lowerBuscar))));
            }

            // por defecto mostrar solo activos, pero si se selecciona "Todos" mostrar todos
            if (string.IsNullOrWhiteSpace(filtroEstado))
            {
                query = query.Where(u => u.Estado == "Activo");
                filtroEstado = "Activo";
            }
            else if (filtroEstado != "Todos")
            {
                query = query.Where(u => u.Estado == filtroEstado);
            }

            ViewData["CurrentBuscar"] = textoBuscar;
            ViewData["CurrentEstado"] = filtroEstado;

            var lista = await query.ToListAsync();
            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> Perfil(long? id, string modo = "create", string rol = "")
        {
            var vm = new PerfilViewModel
            {
                Modo = modo,
                RolSeleccionado = rol
            };

            if (id.HasValue && modo == "edit")
            {
                var usr = await _context.Usuarios
                    .Include(u => u.Persona)
                    .Include(u => u.Estudiante)
                    .Include(u => u.Juece)
                    .Include(u => u.Tutore)
                    .FirstOrDefaultAsync(u => u.UsuarioId == id.Value);

                if (usr == null) return NotFound();

                vm.UsuarioId = usr.UsuarioId;
                vm.PersonaId = usr.PersonaId;
                vm.EstadoUsuario = usr.Estado ?? "Activo";
                vm.Correo = usr.Correo;

                if (usr.Persona != null)
                {
                    vm.Documento = usr.Persona.Documento;
                    vm.Nombres = usr.Persona.Nombres;
                    vm.Apellidos = usr.Persona.Apellidos;
                    vm.Telefono = usr.Persona.Telefono;
                    vm.FechaNacimiento = usr.Persona.FechaNacimiento;
                    vm.Sexo = usr.Persona.Sexo;
                    vm.Nacionalidad = usr.Persona.Nacionalidad;
                }

                if (usr.Estudiante != null) // cambiado para que no se asuma que si no es estudiante es tutor, ya que puede ser juez o coord
                {
                    vm.RolSeleccionado = "estudiante";
                    vm.Grado = usr.Estudiante.Grado != null ? int.Parse(usr.Estudiante.Grado) : null;
                }
                else if (usr.Tutore != null)
                {
                    vm.RolSeleccionado = "tutor";
                    vm.Especialidad = usr.Tutore.Especialidad;
                }
                else if (usr.Juece != null)
                {
                    vm.RolSeleccionado = "juez";
                }
                else
                {
                    vm.RolSeleccionado = "coord";
                }
            }
            
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarPerfil(PerfilViewModel model)
        {
            // si edita no se requiere validar la clave, ya que no siempre se va a cambiar, pero si es creación si se requiere
            if (model.Modo == "edit")
            {
                ModelState.Remove("Clave");
            }

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewData["ErroresModelState"] = string.Join(" | ", errores);
                return View("Perfil", model);
            }

            Persona per;
            Usuario usr;

            // Encriptación manual (SHA-256)
            string hashClave = null;
            if (!string.IsNullOrEmpty(model.Clave))
            {
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var bytes = System.Text.Encoding.UTF8.GetBytes(model.Clave);
                    var hashBytes = sha256.ComputeHash(bytes);
                    hashClave = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (model.Modo == "edit" && model.UsuarioId.HasValue)
                {
                    // actualizar tanto Usuario como Persona
                    usr = await _context.Usuarios
                        .Include(u => u.Persona)
                        .FirstOrDefaultAsync(u => u.UsuarioId == model.UsuarioId.Value);

                    if (usr == null) return NotFound();

                    per = usr.Persona;
                    if (per != null)
                    {
                        per.Documento = model.Documento;
                        per.Nombres = model.Nombres;
                        per.Apellidos = model.Apellidos;
                        per.Telefono = model.Telefono;
                        per.FechaNacimiento = model.FechaNacimiento;
                        per.Sexo = model.Sexo;
                        per.Nacionalidad = model.Nacionalidad;
                        per.FechaModificacion = DateTime.Now;
                        _context.Personas.Update(per);
                    }

                    usr.Correo = model.Correo;
                    if (hashClave != null) usr.PasswordHash = hashClave;
                    usr.Estado = model.EstadoUsuario;
                    usr.FechaModificacion = DateTime.Now;
                    _context.Usuarios.Update(usr);
                }
                else
                {
                    //crear nuevo Usuario y Persona
                    per = new Persona
                    {
                        Documento = model.Documento,
                        Nombres = model.Nombres,
                        Apellidos = model.Apellidos,
                        Telefono = model.Telefono,
                        FechaNacimiento = model.FechaNacimiento,
                        Sexo = model.Sexo,
                        Nacionalidad = model.Nacionalidad,
                        FechaCreacion = DateTime.Now
                    };

                    _context.Personas.Add(per);
                    await _context.SaveChangesAsync(); // Se necesita el PersonaId

                    usr = new Usuario
                    {
                        PersonaId = per.PersonaId,
                        Correo = model.Correo,
                        PasswordHash = hashClave ?? "",
                        Estado = model.EstadoUsuario,
                        FechaCreacion = DateTime.Now
                    };
                    _context.Usuarios.Add(usr);
                    await _context.SaveChangesAsync(); // Se necesita el UsuarioId
                }

                // Mapeos específicos de Rol
                await LimpiarRolesAnteriores(usr.UsuarioId);

                string rolLower = model.RolSeleccionado?.ToLower() ?? "";
                
                if (rolLower == "estudiante")
                {
                    _context.Estudiantes.Add(new Estudiante { EstudianteId = usr.UsuarioId, Grado = model.Grado?.ToString() });
                }
                else if (rolLower == "tutor")
                {
                    _context.Tutores.Add(new Tutore { TutorId = usr.UsuarioId, Especialidad = model.Especialidad });
                }
                else if (rolLower == "juez")
                {
                    _context.Jueces.Add(new Juece { JuezId = usr.UsuarioId });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Retornar a una lista
                if (rolLower == "estudiante") return RedirectToAction("Index", "Estudiantes");
                if (rolLower == "tutor") return RedirectToAction("Index", "Tutores");
                if (rolLower == "juez") return RedirectToAction("Index", "Jueces");
                return RedirectToAction("Index", "Usuarios");
            }
            catch (Exception ex)
            {
                try { await transaction.RollbackAsync(); } catch { /* en caso de error, intentar hacer rollback, pero si falla el rollback no hacer nada para no enmascarar el error original */ }

                // limpiar el ChangeTracker para evitar que queden entidades en estado modificado o agregado después del error
                _context.ChangeTracker.Clear();

                // buscar en la cadena de excepciones internas si hay un mensaje que contenga "Duplicate entry" para dar un mensaje de error más amigable
                var current = ex;
                string innerMsg = ex.Message;
                while (current != null)
                {
                    if (current.Message.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase))
                    {
                        innerMsg = current.Message;
                        break;
                    }
                    current = current.InnerException;
                }

                string errorUsuario;
                if (innerMsg.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase))
                {
                    if (innerMsg.Contains("correo", StringComparison.OrdinalIgnoreCase))
                        errorUsuario = "Ya existe un usuario con ese correo electrónico.";
                    else if (innerMsg.Contains("documento", StringComparison.OrdinalIgnoreCase))
                        errorUsuario = "Ya existe una persona con ese número de documento.";
                    else
                        errorUsuario = "Ya existe un registro con datos duplicados. Verifique los campos únicos.";
                }
                else
                {
                    errorUsuario = "Ocurrió un error al guardar los datos. Por favor intente de nuevo.";
                }

                ViewData["ErroresModelState"] = errorUsuario;
                return View("Perfil", model);
            }
        }

        private async Task LimpiarRolesAnteriores(long usuarioId)
        {
            var est = await _context.Estudiantes.FindAsync(usuarioId);
            if (est != null) _context.Estudiantes.Remove(est);

            var tut = await _context.Tutores.FindAsync(usuarioId);
            if (tut != null) _context.Tutores.Remove(tut);

            var jue = await _context.Jueces.FindAsync(usuarioId);
            if (jue != null) _context.Jueces.Remove(jue);

            await _context.SaveChangesAsync();
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return RedirectToAction("Perfil", new { modo = "create" });
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();
            return RedirectToAction("Perfil", new { id, modo = "edit" });
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioCreacionNavigation)
                .Include(u => u.UsuarioModificacionNavigation)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Persona)
                .Include(u => u.UsuarioCreacionNavigation)
                .Include(u => u.UsuarioModificacionNavigation)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(long id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}