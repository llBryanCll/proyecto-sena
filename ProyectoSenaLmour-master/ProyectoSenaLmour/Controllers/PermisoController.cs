using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoSenaLmour.Models;

namespace ProyectoSenaLmour.Controllers
{
    public class PermisoController : Controller
    {
        private readonly LmourContext _context;

        public PermisoController(LmourContext context)
        {
            _context = context;
        }


        public class PermisosController : Controller
        {
            // Método para cargar los permisos del usuario
            public IActionResult Index()
            {
                // Aquí cargarías los permisos del usuario desde la base de datos y los pasarías a la vista
                IEnumerable<Permiso> permisos = ObtenerPermisosUsuario(); // Implementa este método según tu lógica
                return View(permisos);
            }

            private IEnumerable<Permiso> ObtenerPermisosUsuario()
            {
                throw new NotImplementedException();
            }

            // Método para guardar los cambios en los permisos
            [HttpPost]
            public IActionResult GuardarCambios(IEnumerable<Permiso> permisos)
            {
                // Aquí guardarías los cambios en la base de datos
                GuardarPermisosUsuario(permisos); // Implementa este método según tu lógica
                return RedirectToAction("Index");
            }

            private void GuardarPermisosUsuario(IEnumerable<Permiso> permisos)
            {
                throw new NotImplementedException();
            }
        }





        // GET: Permisoes
        public async Task<IActionResult> Index()
        {
              return _context.Permisos != null ? 
                          View(await _context.Permisos.ToListAsync()) :
                          Problem("Entity set 'LmourContext.Permisos'  is null.");
        }

        // GET: Permisoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Permisos == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .FirstOrDefaultAsync(m => m.IdPermiso == id);
            if (permiso == null)
            {
                return NotFound();
            }

            return View(permiso);
        }

        // GET: Permisoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Permisoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPermiso,NomPermiso")] Permiso permiso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permiso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(permiso);
        }

        // GET: Permisoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Permisos == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }
            return View(permiso);
        }

        // POST: Permisoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPermiso,NomPermiso")] Permiso permiso)
        {
            if (id != permiso.IdPermiso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permiso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermisoExists(permiso.IdPermiso))
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
            return View(permiso);
        }

        // GET: Permisoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Permisos == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .FirstOrDefaultAsync(m => m.IdPermiso == id);
            if (permiso == null)
            {
                return NotFound();
            }

            return View(permiso);
        }

        // POST: Permisoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Permisos == null)
            {
                return Problem("Entity set 'LmourContext.Permisos'  is null.");
            }
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso != null)
            {
                _context.Permisos.Remove(permiso);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermisoExists(int id)
        {
          return (_context.Permisos?.Any(e => e.IdPermiso == id)).GetValueOrDefault();
        }
    }
}
