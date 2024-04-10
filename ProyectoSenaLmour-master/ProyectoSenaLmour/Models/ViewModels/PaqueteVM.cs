using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ProyectoSenaLmour.Models.ViewModels
{
    public class PaqueteVM
    {
        public Paquete oPaquete { get; set; }
        public List<SelectListItem> oListaNombreHabitacion { get; set; }
        public List<int> ServiciosSeleccionados { get; set; } // Propiedad para almacenar los IDs de los servicios seleccionados
    }
}
