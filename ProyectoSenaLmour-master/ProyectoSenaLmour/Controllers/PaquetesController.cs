using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoSenaLmour.Models;
using ProyectoSenaLmour.Models.ViewModels;

namespace ProyectoSenaLmour.Controllers
{
    public class PaquetesController : Controller
    {
        private readonly LmourContext _context;

        public PaquetesController(LmourContext context)
        {
            _context = context;
        }

        // GET: Paquetes
        public IActionResult Index()
        {
            var paquetes = _context.Paquetes
               .Include(p => p.IdHabitacionNavigation)
               .Include(p => p.PaqueteServicios)
                   .ThenInclude(ps => ps.IdServicioNavigation)
                .ToList();

            return View(paquetes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado)
        {
            var paquete = await _context.Paquetes.FindAsync(id);
            if (paquete == null)
            {
                return NotFound();
            }

            // Cambiar el estado del paquete
            paquete.Estado = nuevoEstado;

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Redirigir de vuelta a la vista de índice
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            PaqueteVM oPaqueteVM = new PaqueteVM()
            {
                oPaquete = new Paquete(),
                oListaNombreHabitacion = _context.Habitaciones.Select(mtp => new SelectListItem()
                {
                    Text = mtp.Nombre,
                    Value = mtp.IdHabitacion.ToString()
                }).ToList(),
            };

            var serviciosDisponibles = _context.Servicios.ToList();

            ViewBag.Servicios = serviciosDisponibles;

            return View(oPaqueteVM);
        }

        [HttpPost]
        public IActionResult Create(PaqueteVM oPaqueteVM)
        {
            // Agregar el paquete a la base de datos
            _context.Paquetes.Add(oPaqueteVM.oPaquete);
            _context.SaveChanges();

            // Guardar los servicios asociados al paquete
            foreach (var servicioId in oPaqueteVM.ServiciosSeleccionados)
            {
                PaqueteServicio paqueteServicio = new PaqueteServicio
                {
                    IdPaquete = oPaqueteVM.oPaquete.IdPaquete,
                    IdServicio = servicioId,
                    // Asignar el costo del servicio si es necesario
                };
                _context.PaqueteServicios.Add(paqueteServicio);
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult ObtenerCostoServicio(int servicioId)
        {
            var CostoServicio = _context.Servicios
                .Where(s => s.IdServicio == servicioId)
                .FirstOrDefault();

            return Json(new
            {
                costo = CostoServicio?.Costo
            });
        }

        // GET: Paquetes/Details/5
        // GET: Paquetes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paquete = await _context.Paquetes
                .Include(p => p.IdHabitacionNavigation)
                .Include(p => p.PaqueteServicios)
                    .ThenInclude(ps => ps.IdServicioNavigation) // Esto carga los servicios asociados al paquete
                .FirstOrDefaultAsync(m => m.IdPaquete == id);

            if (paquete == null)
            {
                return NotFound();
            }

            // Cargar explícitamente las propiedades de navegación de PaqueteServicio si no se cargaron anteriormente
            foreach (var paqueteServicio in paquete.PaqueteServicios)
            {
                if (paqueteServicio.IdServicioNavigation == null)
                {
                    _context.Entry(paqueteServicio).Reference(ps => ps.IdServicioNavigation).Load();
                }
            }

            return View(paquete);
        }




        // POST: Paquetes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       

        // GET: Paquetes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Paquetes == null)
            {
                return NotFound();
            }

            var paquete = await _context.Paquetes
                .Include(p => p.IdHabitacionNavigation)
                .FirstOrDefaultAsync(m => m.IdPaquete == id);
            if (paquete == null)
            {
                return NotFound();
            }

            return View(paquete);
        }

        // POST: Paquetes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Paquetes == null)
            {
                return Problem("Entity set 'LmourContext.Paquetes'  is null.");
            }
            var paquete = await _context.Paquetes.FindAsync(id);
            if (paquete != null)
            {
                _context.Paquetes.Remove(paquete);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaqueteExists(int id)

        {
            return (_context.Paquetes?.Any(e => e.IdPaquete == id)).GetValueOrDefault();
        }

        // GET: Paquetes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paquete = await _context.Paquetes
                .Include(p => p.PaqueteServicios)
                .FirstOrDefaultAsync(m => m.IdPaquete == id);

            if (paquete == null)
            {
                return NotFound();
            }

            // Cargar explícitamente las propiedades de navegación de PaqueteServicio si no se cargaron anteriormente
            foreach (var paqueteServicio in paquete.PaqueteServicios)
            {
                if (paqueteServicio.IdServicioNavigation == null)
                {
                    _context.Entry(paqueteServicio).Reference(ps => ps.IdServicioNavigation).Load();
                }
            }

            // Cargar las listas necesarias para mostrar en el formulario de edición, por ejemplo, las habitaciones y servicios disponibles
            var habitaciones = _context.Habitaciones.Select(mtp => new SelectListItem()
            {
                Text = mtp.Nombre,
                Value = mtp.IdHabitacion.ToString()
            }).ToList();

            ViewBag.Habitaciones = habitaciones;

            var servicios = _context.Servicios.ToList();
            ViewBag.Servicios = servicios;

            return View(paquete);
        }

        // POST: Paquetes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPaquete,NomPaquete,Costo,IdHabitacion,Estado,Descripcion")] Paquete paquete, List<int> ServiciosSeleccionados)
        {
            if (id != paquete.IdPaquete)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paquete);
                    await _context.SaveChangesAsync();

                    // Eliminar los servicios asociados anteriores
                    var serviciosAnteriores = _context.PaqueteServicios.Where(ps => ps.IdPaquete == paquete.IdPaquete);
                    _context.PaqueteServicios.RemoveRange(serviciosAnteriores);
                    await _context.SaveChangesAsync();

                    // Guardar los nuevos servicios asociados al paquete
                    foreach (var servicioId in ServiciosSeleccionados)
                    {
                        PaqueteServicio paqueteServicio = new PaqueteServicio
                        {
                            IdPaquete = paquete.IdPaquete,
                            IdServicio = servicioId
                        };
                        _context.PaqueteServicios.Add(paqueteServicio);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaqueteExists(paquete.IdPaquete))
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
            return View(paquete);
        }

    }
}



   