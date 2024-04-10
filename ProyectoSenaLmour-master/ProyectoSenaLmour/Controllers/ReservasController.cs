using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
                .Include(r => r.NroDocumentoUsuarioNavigation)
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


        public IActionResult Anular(int id)
        {
            var reserva = _context.Reservas
                .FirstOrDefault(m => m.IdReserva == id);

            reserva.IdEstadoReserva = 5;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private bool ReservaExistsForAnular(int id)
        {
            return (_context.Reservas?.Any(e => e.IdReserva == id)).GetValueOrDefault();
        }
    }
}
