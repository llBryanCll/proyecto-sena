﻿using System;
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
        public IActionResult index()
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
            _context.Paquetes.Add(oPaqueteVM.oPaquete);
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

            return View(paquete);
        }



        // POST: Paquetes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPaquete,NomPaquete,Costo,IdHabitacion,Estado,Descripcion")] Paquete paquete)
        {
            if (id != paquete.IdPaquete)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(paquete).State = EntityState.Modified; ;
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
            ViewData["IdHabitacion"] = new SelectList(_context.Habitaciones, "IdHabitacion", "IdHabitacion", paquete.IdHabitacion);
            return View(paquete);

        }

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
    }
}



   