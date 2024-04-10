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
    public class TipoHabitacionesController : Controller
    {
        private readonly LmourContext _context;

        public TipoHabitacionesController(LmourContext context)
        {
            _context = context;
        }

        // GET: TipoHabitaciones
        public async Task<IActionResult> Index()
        {
            return _context.TipoHabitaciones != null ?
                       View(await _context.TipoHabitaciones.ToListAsync()) :
                       Problem("Entity set 'LmourContext.TipoHabitaciones'  is null.");
        }

        // GET: TipoHabitaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TipoHabitaciones == null)
            {
                return NotFound();
            }

            var tipoHabitacione = await _context.TipoHabitaciones
                .FirstOrDefaultAsync(m => m.IdTipoHabitacion == id);
            if (tipoHabitacione == null)
            {
                return NotFound();
            }

            return View(tipoHabitacione);
        }

        // GET: TipoHabitaciones/Create
        public IActionResult Create()
        {
            ViewData["Estados"] = new SelectList(new[] { "Activo", "Inactivo" });
            return View();
        }

        // POST: TipoHabitaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoHabitacion,NomTipoHabitacion,NumeroPersonas,Estado")] TipoHabitacione tipoHabitacione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoHabitacione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoHabitacione);
        }

        // GET: TipoHabitaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoHabitacion = await _context.TipoHabitaciones.FindAsync(id);
            if (tipoHabitacion == null)
            {
                return NotFound();
            }

            // Obtener la lista de nombres de tipos de habitaciones existentes
            //var nombresTiposHabitacionesExistentes = await _context.TipoHabitaciones
            //    .Where(th => th.IdTipoHabitacion != id) // Excluir el tipo de habitación actual
            //    .Select(th => th.NomTipoHabitacion)
            //    .ToListAsync();

            // Crear una lista desplegable con los nombres de tipos de habitaciones existentes
            //ViewData["NomTipoHabitacion"] = new SelectList(nombresTiposHabitacionesExistentes);
            ViewData["Estados"] = new SelectList(new[] { "Activo", "Inactivo" });
            return View(tipoHabitacion);
        }

        // POST: TipoHabitaciones/Edit/5
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("IdTipoHabitacion,NomTipoHabitacion,NumeroPersonas,Estado")] TipoHabitacione tipoHabitacion)
{
    if (id != tipoHabitacion.IdTipoHabitacion)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            var tipoHabitacionOriginal = await _context.TipoHabitaciones.FindAsync(id);
            if (tipoHabitacionOriginal == null)
            {
                return NotFound();
            }

            // Actualizar los campos de la entidad original con los valores del formulario
            tipoHabitacionOriginal.NomTipoHabitacion = tipoHabitacion.NomTipoHabitacion;
            tipoHabitacionOriginal.NumeroPersonas = tipoHabitacion.NumeroPersonas;
            tipoHabitacionOriginal.Estado = tipoHabitacion.Estado;

            // Actualizar la entidad original en el contexto
            _context.Update(tipoHabitacionOriginal);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Redirigir al índice después de guardar los cambios
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            // Manejar excepciones de concurrencia aquí si es necesario
            if (!TipoHabitacionExists(tipoHabitacion.IdTipoHabitacion))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }
    return View(tipoHabitacion);
}

        private bool TipoHabitacionExists(int idTipoHabitacion)
        {
            throw new NotImplementedException();
        }

        // GET: TipoHabitaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TipoHabitaciones == null)
            {
                return NotFound();
            }

            var tipoHabitacione = await _context.TipoHabitaciones
                .FirstOrDefaultAsync(m => m.IdTipoHabitacion == id);
            if (tipoHabitacione == null)
            {
                return NotFound();
            }

            return View(tipoHabitacione);
        }

        // POST: TipoHabitaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TipoHabitaciones == null)
            {
                return Problem("Entity set 'LmourContext.TipoHabitaciones'  is null.");
            }
            var tipoHabitacione = await _context.TipoHabitaciones.FindAsync(id);
            if (tipoHabitacione != null)
            {
                _context.TipoHabitaciones.Remove(tipoHabitacione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: TipoHabitaciones/CambiarEstado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado)
        {
            var tipoHabitacione = await _context.TipoHabitaciones.FindAsync(id);
            if (tipoHabitacione == null)
            {
                return NotFound();
            }

            tipoHabitacione.Estado = nuevoEstado;
            _context.Update(tipoHabitacione);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TipoHabitacioneExists(int id)
        {
            return (_context.TipoHabitaciones?.Any(e => e.IdTipoHabitacion == id)).GetValueOrDefault();
        }
    }
}
