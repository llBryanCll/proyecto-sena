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
            // Guardar la reserva
            _context.Reservas.Add(oReservaVM.oReserva);
            _context.SaveChanges();

            // Obtener el ID de la reserva recién guardada
            int reservaId = oReservaVM.oReserva.IdReserva;

            // Guardar el detalle del paquete seleccionado
            if (!string.IsNullOrEmpty(paqueteSeleccionado))
            {
                int paqueteId = int.Parse(paqueteSeleccionado);
                DetalleReservaPaquete detallePaquete = new DetalleReservaPaquete
                {
                    IdReserva = reservaId,
                    IdPaquete = paqueteId
                };
                _context.DetalleReservaPaquetes.Add(detallePaquete);
            }

            // Guardar los detalles de los servicios seleccionados
            if (!string.IsNullOrEmpty(serviciosSeleccionados))
            {
                string[] servicioIds = serviciosSeleccionados.Split(',');
                foreach (var servicioIdString in servicioIds)
                {
                    int servicioId = int.Parse(servicioIdString);
                    DetalleReservaServicio detalleServicio = new DetalleReservaServicio
                    {
                        IdReserva = reservaId,
                        IdServicio = servicioId
                    };
                    _context.DetalleReservaServicios.Add(detalleServicio);
                }
            }

            // Guardar los cambios en la base de datos
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        //[HttpPost]
        //public IActionResult Create(ReservaVM oReservaVM, string paqueteSeleccionado, string serviciosSeleccionados)
        //{

        //    _context.Reservas.Add(oReservaVM.oReserva);
        //    _context.SaveChanges();


        //    return RedirectToAction("Index");
        //}

        //public IActionResult ObtenerCostoPaquete(int paqueteId)
        //{
        //    var CostoPaquete = _context.Paquetes
        //        .Where(p => p.IdPaquete == paqueteId)
        //        .FirstOrDefault();

        //    return Json(new
        //    {
        //        costo = CostoPaquete.Costo
        //    });
        //}

        //public IActionResult ObtenerCostoServicio(int servicioId)
        //{
        //    var CostoServicio = _context.Servicios
        //        .Where(s => s.IdServicio == servicioId)
        //        .FirstOrDefault();

        //    return Json(new
        //    {
        //        costo = CostoServicio.Costo
        //    });
        //}


        // GET: Reservas/Detalle/
        public async Task<IActionResult> Details(int id)
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

            if (oReservaVM == null)
            {
                return NotFound();
            }

            return View(oReservaVM);
        }


        // GET: Reservas/Edit/5
        public IActionResult Edit(int id)
        {
            // Buscar la reserva que se va a editar por su ID
            var reservaEditar = _context.Reservas
                .Include(r => r.IdEstadoReservaNavigation)
                .Include(r => r.NroDocumentoClienteNavigation)
                .FirstOrDefault(r => r.IdReserva == id);

            if (reservaEditar == null)
            {
                return NotFound(); // Si no se encuentra la reserva, retornar un error 404
            }

            // Construir el ViewModel de Reserva para pasar a la vista
            ReservaVM oReservaVM = new ReservaVM()
            {
                oReserva = reservaEditar,
                oListaEstados = _context.EstadosReservas.Select(reserva => new SelectListItem()
                {
                    Text = reserva.NombreEstadoReserva,
                    Value = reserva.IdEstadoReserva.ToString(),
                    Selected = reserva.IdEstadoReserva == reservaEditar.IdEstadoReserva // Marcar como seleccionado el estado actual de la reserva
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

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Edit(int id, ReservaVM oReservaVM, string paqueteSeleccionado, string serviciosSeleccionados)
        {
            if (id != oReservaVM.oReserva.IdReserva)
            {
                return NotFound(); // Si el ID de la reserva no coincide con el ID recibido, retornar un error 404
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualizar la reserva en el contexto y guardar los cambios
                    _context.Update(oReservaVM.oReserva);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(oReservaVM.oReserva.IdReserva))
                    {
                        return NotFound(); // Si la reserva no existe, retornar un error 404
                    }
                    else
                    {
                        throw; // Si hay un error de concurrencia, lanzar una excepción
                    }
                }
                return RedirectToAction("Index");
            }

            // Si llegamos aquí, significa que hubo un error en el modelo, volver a cargar la vista con los datos de la reserva
            return View(oReservaVM);
        }

        // Método auxiliar para verificar si una reserva existe
        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.IdReserva == id);
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
