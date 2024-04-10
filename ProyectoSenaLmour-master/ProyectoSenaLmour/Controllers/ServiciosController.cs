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
    public class ServiciosController : Controller
    {
        private readonly LmourContext _context;

        public ServiciosController(LmourContext context)
        {
            _context = context;
        }

        // GET: Servicios
        public async Task<IActionResult> Index()
        {
            var LmourContext = _context.Servicios.Include(h => h.IdTipoServicioNavigation);
            return View(await LmourContext.ToListAsync());
        }

        // GET: Habitaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Servicios == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios
                .Include(h => h.IdTipoServicioNavigation)
                .FirstOrDefaultAsync(m => m.IdServicio == id);
            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }

        // GET: Servicios/Create
        public IActionResult Create()
        {
            ViewData["IdTipoServicio"] = new SelectList(_context.TipoServicios, "IdTipoServicio", "NombreTipoServicio");
            ViewData["Estados"] = new SelectList(new[] { "Activo", "Inactivo" });
            return View();
        }

        // POST: Habitaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idservicio,IdTipoServicio,NomServicio,IdTipoServicio,Estado,Descripcion,Costo")] Servicio servicio)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(habitacione);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            _context.Add(servicio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["IdTipoServicio"] = new SelectList(_context.TipoServicios, "IdTipoServicio", "NombreTipoServicio", servicio.IdTipoServicio);
            return View(servicio);
        }

        // GET: Servicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Servicios == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }

            var nombresExistentes = await _context.Servicios
                .Where(h => h.IdServicio != id) // Excluir la habitación actual
                .Select(h => h.NomServicio)
                .ToListAsync();

            ViewData["NomServicio"] = new SelectList(nombresExistentes, Servicio.Equals);
            ViewData["IdTipoServicio"] = new SelectList(_context.TipoServicios, "IdTipoServicio", "NombreTipoServicio");
            ViewData["Estados"] = new SelectList(new[] { "Activo", "Inactivo" });
            return View(servicio);
        }

        // POST: Servicios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdServicio,IdTipoServicio,NomServicio,Costo,Descripcion,Estado")] Servicio servicio)
        {
            if (id != servicio.IdServicio)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(servicio).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicioExists(servicio.IdServicio))
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
            ViewData["IdTipoServicio"] = new SelectList(_context.TipoServicios, "IdTipoServicio", "IdTipoServicio", servicio.IdTipoServicio);
            return View(servicio);
        }

        // GET: Servicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Servicios == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios
                .Include(s => s.IdTipoServicioNavigation)
                .FirstOrDefaultAsync(m => m.IdServicio == id);
            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }

        // POST: Servicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Servicios == null)
            {
                return Problem("Entity set 'LmourContext.Servicios'  is null.");
            }
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio != null)
            {
                _context.Servicios.Remove(servicio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Servicios/CambiarEstado/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }

            servicio.Estado = nuevoEstado;

            try
            {
                _context.Entry(servicio).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicioExists(servicio.IdServicio))
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

        private bool ServicioExists(int id)
        {
            return (_context.Servicios?.Any(e => e.IdServicio == id)).GetValueOrDefault();
        }
    }
}
