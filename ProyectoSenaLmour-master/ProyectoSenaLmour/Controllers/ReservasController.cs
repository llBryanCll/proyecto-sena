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
    public class ReservasController : Controller
    {
        private readonly LmourContext _context;

        public ReservasController(LmourContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var reservas = _context.Reservas
                .Include(r => r.IdEstadoReservaNavigation)
                .Include(r => r.NroDocumentoClienteNavigation)
                .ToList();

            return View(reservas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ReservaVM oReservaVM = new ReservaVM()
            {
                oReserva = new Reserva(),
                oListaEstados = _context.EstadosReservas.Select(reserva => new SelectListItem()
                {
                    Text = reserva.NombreEstadoReserva,
                    Value = reserva.IdEstadoReserva.ToString()
                }).ToList(),
                oListaMetodosPago = _context.MetodoPagos.Select(mtp => new SelectListItem()
                {
                    Text = mtp.NomMetodoPago,
                    Value = mtp.IdMetodoPago.ToString()
                }).ToList(),
            };

            var paqueteDisponibles = _context.Paquetes.ToList();
            var serviciosDisponibles = _context.Servicios.ToList();

            ViewBag.Paquetes = paqueteDisponibles;
            ViewBag.Servicios = serviciosDisponibles;

            return View(oReservaVM);



        }

        [HttpPost]
        public IActionResult Create(ReservaVM oReservaVM, string paqueteSeleccionado, string serviciosSeleccionados)
        {
            _context.Reservas.Add(oReservaVM.oReserva);
            _context.SaveChanges();


            return RedirectToAction("Index");
        }

        public IActionResult ObtenerCostoPaquete(int paqueteId)
        {
            var CostoPaquete = _context.Paquetes
                .Where(p => p.IdPaquete == paqueteId)
                .FirstOrDefault();

            return Json(new
            {
                costo = CostoPaquete.Costo 
            });
        }

        public IActionResult ObtenerCostoServicio(int servicioId)
        {
            var CostoServicio = _context.Servicios
                .Where(s => s.IdServicio == servicioId)
                .FirstOrDefault();

            return Json(new
            {
                costo = CostoServicio.Costo
            });
        }



        //// GET: Reservas
        //public async Task<IActionResult> Index()
        //{
        //    var lmourContext = _context.Reservas.Include(r => r.IdEstadoReservaNavigation).Include(r => r.IdMetodoPagoNavigation).Include(r => r.NroDocumentoClienteNavigation).Include(r => r.NroDocumentoUsuarioNavigation);
        //    return View(await lmourContext.ToListAsync());
        //}

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.IdEstadoReservaNavigation)
                .Include(r => r.IdMetodoPagoNavigation)
                .Include(r => r.NroDocumentoClienteNavigation)
                .Include(r => r.NroDocumentoUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdReserva == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        //public IActionResult Create()
        //{
        //    ViewBag.MetodoPago = _context.MetodoPagos.Select(metodoPago => new SelectListItem()
        //    {
        //        Text = metodoPago.NomMetodoPago,
        //        Value = metodoPago.IdMetodoPago.ToString()
        //    }).ToList();
        //    ViewBag.IdEstadoReserva = _context.EstadosReservas.Select(estadoReserva => new SelectListItem()
        //    {
        //        Text = estadoReserva.NombreEstadoReserva,
        //        Value = estadoReserva.IdEstadoReserva.ToString()
        //    }).ToList();
        //    return View();
        //}

        //// POST: Reservas/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("IdReserva,NroDocumentoCliente,NroDocumentoUsuario,FechaReserva,FechaInicio,FechaFinalizacion,SubTotal,Descuento,Iva,MontoTotal,IdMetodoPago,NroPersonas,IdEstadoReserva")] Reserva reserva)
        //{
        //    //if (ModelState.IsValid)
        //    {
        //        _context.Add(reserva);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["IdEstadoReserva"] = new SelectList(_context.EstadosReservas, "IdEstadoReserva", "IdEstadoReserva", reserva.IdEstadoReserva);
        //    ViewData["MetodoPago"] = new SelectList(_context.MetodoPagos, "IdMetodoPago", "IdMetodoPago", reserva.IdMetodoPago);
        //    ViewData["NroDocumentoCliente"] = new SelectList(_context.Clientes, "NroDocumento", "NroDocumento", reserva.NroDocumentoCliente);
        //    ViewData["NroDocumentoUsuario"] = new SelectList(_context.Usuarios, "NroDocumento", "NroDocumento", reserva.NroDocumentoUsuario);
        //    return View(reserva);
        //}

        // GET: Reservas/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Reservas == null)
        //    {
        //        return NotFound();
        //    }

        //    var reserva = await _context.Reservas.FindAsync(id);
        //    if (reserva == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["IdEstadoReserva"] = new SelectList(_context.EstadosReservas, "IdEstadoReserva", "IdEstadoReserva", reserva.IdEstadoReserva);
        //    ViewData["MetodoPago"] = new SelectList(_context.MetodoPagos, "IdMetodoPago", "IdMetodoPago", reserva.IdMetodoPago);
        //    ViewData["NroDocumentoCliente"] = new SelectList(_context.Clientes, "NroDocumento", "NroDocumento", reserva.NroDocumentoCliente);
        //    ViewData["NroDocumentoUsuario"] = new SelectList(_context.Usuarios, "NroDocumento", "NroDocumento", reserva.NroDocumentoUsuario);
        //    return View(reserva);
        //}
        public IActionResult Edit()
        {
            ReservaVM oReservaVM = new ReservaVM()
            {
                oReserva = new Reserva(),
                oListaEstados = _context.EstadosReservas.Select(reserva => new SelectListItem()
                {
                    Text = reserva.NombreEstadoReserva,
                    Value = reserva.IdEstadoReserva.ToString()
                }).ToList(),
                oListaMetodosPago = _context.MetodoPagos.Select(mtp => new SelectListItem()
                {
                    Text = mtp.NomMetodoPago,
                    Value = mtp.IdMetodoPago.ToString()
                }).ToList(),
            };

            var paqueteDisponibles = _context.Paquetes.ToList();
            var serviciosDisponibles = _context.Servicios.ToList();

            ViewBag.Paquetes = paqueteDisponibles;
            ViewBag.Servicios = serviciosDisponibles;

            return View(oReservaVM);
        }

        [HttpPost]
        public IActionResult Edit(ReservaVM oReservaVM, string paqueteSeleccionado, string serviciosSeleccionados)
        {
            _context.Reservas.Add(oReservaVM.oReserva);
            _context.SaveChanges();


            return RedirectToAction("Index");
        }

        public IActionResult ObtenerCostoPaquete_2(int paqueteId)
        {
            var CostoPaquete = _context.Paquetes
                .Where(p => p.IdPaquete == paqueteId)
                .FirstOrDefault();

            return Json(new
            {
                costo = CostoPaquete.Costo
            });
        }

        public IActionResult ObtenerCostoServicio_2(int servicioId)
        {
            var CostoServicio = _context.Servicios
                .Where(s => s.IdServicio == servicioId)
                .FirstOrDefault();

            return Json(new
            {
                costo = CostoServicio.Costo
            });
        }


        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("IdReserva,NroDocumentoCliente,NroDocumentoUsuario,FechaReserva,FechaInicio,FechaFinalizacion,SubTotal,Descuento,Iva,MontoTotal,MetodoPago,NroPersonas,IdEstadoReserva")] Reserva reserva)
        //{
        //    if (id != reserva.IdReserva)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(reserva);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ReservaExists(reserva.IdReserva))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["IdEstadoReserva"] = new SelectList(_context.EstadosReservas, "IdEstadoReserva", "IdEstadoReserva", reserva.IdEstadoReserva);
        //    ViewData["MetodoPago"] = new SelectList(_context.MetodoPagos, "IdMetodoPago", "IdMetodoPago", reserva.IdMetodoPago);
        //    ViewData["NroDocumentoCliente"] = new SelectList(_context.Clientes, "NroDocumento", "NroDocumento", reserva.NroDocumentoCliente);
        //    ViewData["NroDocumentoUsuario"] = new SelectList(_context.Usuarios, "NroDocumento", "NroDocumento", reserva.NroDocumentoUsuario);
        //    return View(reserva);
        //}

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.IdEstadoReservaNavigation)
                .Include(r => r.IdMetodoPagoNavigation)
                .Include(r => r.NroDocumentoClienteNavigation)
                .Include(r => r.NroDocumentoUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdReserva == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservas == null)
            {
                return Problem("Entity set 'LmourContext.Reservas'  is null.");
            }
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return (_context.Reservas?.Any(e => e.IdReserva == id)).GetValueOrDefault();
        }
    }
}
