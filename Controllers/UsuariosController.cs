using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionFerias_CTPINVU.Data;
using GestionFerias_CTPINVU.Models;

namespace GestionFerias_CTPINVU.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Usuarios.Include(u => u.UsuarioCreacionNavigation).Include(u => u.UsuarioModificacionNavigation);
            return View(await appDbContext.ToListAsync());
        }
        public async Task<IActionResult> Perfil()
        {
            var appDbContext = _context.Usuarios.Include(u => u.UsuarioCreacionNavigation).Include(u => u.UsuarioModificacionNavigation);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
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

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["UsuarioCreacion"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId");
            ViewData["UsuarioModificacion"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,Correo,PasswordHash,Estado,UltimoAcceso,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioCreacion"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", usuario.UsuarioCreacion);
            ViewData["UsuarioModificacion"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", usuario.UsuarioModificacion);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["UsuarioCreacion"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", usuario.UsuarioCreacion);
            ViewData["UsuarioModificacion"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", usuario.UsuarioModificacion);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("UsuarioId,Correo,PasswordHash,Estado,UltimoAcceso,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioCreacion"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", usuario.UsuarioCreacion);
            ViewData["UsuarioModificacion"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", usuario.UsuarioModificacion);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(long? id)
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

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(long id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
