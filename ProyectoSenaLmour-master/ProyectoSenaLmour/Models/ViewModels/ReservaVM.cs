using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoSenaLmour.Models.ViewModels
{
    public class ReservaVM
    {
        public Reserva oReserva { get; set; }
        public List<SelectListItem> oListaEstados { get; set; }
        public List<SelectListItem> oListaMetodosPago { get; set; }
    }
}
