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
    public class HabitacionesController : Controller
    {
        private readonly LmourContext _context;

        public HabitacionesController(LmourContext context)
        {
            _context = context;
        }

        // GET: Habitaciones
        public async Task<IActionResult> Index()
        {
            return _context.Habitaciones != null ?
                          View(await _context.Habitaciones.ToListAsync()) :
                          Problem("Entity set 'LmourContext.Habitaciones' is null.");
        }

        // GET: Habitaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Habitaciones == null)
            {
                return NotFound();
            }

            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(m => m.IdHabitacion == id);
            if (habitacion == null)
            {
                return NotFound();
            }

            return View(habitacion);
        }

        // GET: Habitaciones/Create
        public IActionResult Create()
        {
            ViewBag.Estados = new SelectList(new[] { "Activo", "Inactivo" });
            ViewBag.IdtipoHabitacion = new SelectList(new[] { "1", "2" });
            return View();
        }

        // POST: Habitaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoHabitacion,IdHabitacion/*,NumeroHabitacion*/,Nombre,Estado,Descripcion,Costo")] Habitacione habitacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(habitacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(habitacion);
        }

     
        // GET: Habitaciones/Edit/5
       

        // GET: Habitaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Habitaciones == null)
            {
                return NotFound();
            }

            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(m => m.IdHabitacion == id);
            if (habitacion == null)
            {
                return NotFound();
            }

            return View(habitacion);
        }

        // POST: Habitaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Habitaciones == null)
            {
                return Problem("Entity set 'LmourContext.Habitaciones'  is null.");
            }
            var habitacion = await _context.Habitaciones.FindAsync(id);
            if (habitacion != null)
            {
                _context.Habitaciones.Remove(habitacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Método para cambiar el estado de la habitación
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado)
        {
            var habitacion = await _context.Habitaciones.FindAsync(id);
            if (habitacion == null)
            {
                return NotFound();
            }

            habitacion.Estado = nuevoEstado;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool HabitacionExists(int id)
        {
            return _context.Habitaciones.Any(e => e.IdHabitacion == id);
        }

        // GET: Habitaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacion = await _context.Habitaciones.FindAsync(id);
            if (habitacion == null)
            {
                return NotFound();
            }

            ViewBag.Estados = new SelectList(new[] { "Activo", "Inactivo" });
            ViewBag.IdtipoHabitacion = new SelectList(new[] { "1", "2" });

            return View(habitacion);
        }

        // POST: Habitaciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHabitacion,IdTipoHabitacion,NumeroHabitacion,Nombre,Estado,Descripcion,Costo")] Habitacione habitacion)
        {
            if (id != habitacion.IdHabitacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(habitacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabitacionExists(habitacion.IdHabitacion))
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
            return View(habitacion);
        }

    }
}
