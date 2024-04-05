using Microsoft.AspNetCore.Mvc.Rendering;


namespace ProyectoSenaLmour.Models.ViewModels
{
    public class PaqueteVM
    {
        public Paquete oPaquete { get; set; }
        public List<SelectListItem> oListaNombreHabitacion { get; set; }
    }
}
