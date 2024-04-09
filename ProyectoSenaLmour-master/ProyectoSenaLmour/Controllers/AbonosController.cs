using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoSenaLmour.Models;

namespace ProyectoSenaLmour.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AbonosController : Controller
    {
        private readonly LmourContext _context;

        public AbonosController(LmourContext context)
        {
            _context = context;
        }

        // GET: Abonoes
        public IActionResult Index(int idreserva)
        {
            var abonoasociado = _context.Abonos.Where(abono => abono.IdReserva == idreserva).ToList();
            ViewBag.IdReserva = idreserva;
            return View(abonoasociado);
        }

        // GET: Abonoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Abonos == null)
            {
                return NotFound();
            }

            var abono = await _context.Abonos
                .Include(a => a.IdReservaNavigation)
                .FirstOrDefaultAsync(m => m.IdAbono == id);
            if (abono == null)
            {
                return NotFound();
            }

            return View(abono);
        }

        // GET: Abonoes/Create
        [HttpGet]
        public IActionResult Create(int idreserva)
        {
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "IdReserva");
            ViewBag.IdReserva = idreserva;
            ViewBag.ValorDeuda = _context.Reservas.Where(r => r.IdReserva == idreserva).Select(r => r.SubTotal).FirstOrDefault();
            var abonos = _context.Abonos.Where(a => a.IdReserva == idreserva && a.Estado == "Registrado").Select(a => a.SubTotal).ToList();
            var valordeuda = _context.Reservas.Where(r => r.IdReserva == idreserva).Select(r => r.SubTotal).FirstOrDefault();
            var pendiente = valordeuda;
            foreach (var abonado in abonos)
            {
                pendiente -= abonado;
            }
            ViewBag.pendiente = pendiente;
            return View();
        }

        // POST: Abonoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAbono,IdReserva,FechaAbono,ValorDeuda,Porcentaje,Pendiente,SubTotal,Iva,CantAbono,Estado")] Abono abono)
        {
            _context.Add(abono);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Abonoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Abonos == null)
            {
                return NotFound();
            }

            var abono = await _context.Abonos.FindAsync(id);
            if (abono == null)
            {
                return NotFound();
            }
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "IdReserva", abono.IdReserva);
            return View(abono);
        }

        // POST: Abonoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAbono,IdReserva,FechaAbono,ValorDeuda,Porcentaje,Pendiente,SubTotal,Iva,CantAbono,Estado")] Abono abono)
        {
            if (id != abono.IdAbono)
            {
                return NotFound();
            }

            try
            {
                _context.Update(abono);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbonoExists(abono.IdAbono))
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

        public IActionResult Anular(int id)
        {
            var abono = _context.Abonos
                .FirstOrDefault(m => m.IdAbono == id);

            abono.Estado = "Anulado";
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private bool AbonoExists(int id)
        {
            return (_context.Abonos?.Any(e => e.IdAbono == id)).GetValueOrDefault();
        }

        //// POST: Abonoes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Abonos == null)
        //    {
        //        return Problem("Entity set 'LmourContext.Abonos'  is null.");
        //    }
        //    var abono = await _context.Abonos.FindAsync(id);
        //    if (abono != null)
        //    {
        //        _context.Abonos.Remove(abono);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


    }
}
