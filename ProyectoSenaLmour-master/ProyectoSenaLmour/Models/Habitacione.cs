using System;
using System.Collections.Generic;

namespace ProyectoSenaLmour.Models;

public partial class Habitacione
{
    public int IdHabitacion { get; set; }

    public int? IdTipoHabitacion { get; set; }

    public string? Nombre { get; set; }

    public string? Estado { get; set; }

    public string? Descripcion { get; set; }

    public double? Costo { get; set; }

    public virtual TipoHabitacione? IdTipoHabitacionNavigation { get; set; }

    public virtual ICollection<Paquete> Paquetes { get; set; } = new List<Paquete>();
}
