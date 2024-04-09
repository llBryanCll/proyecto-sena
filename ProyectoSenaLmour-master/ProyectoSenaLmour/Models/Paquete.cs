using System;
using System.Collections.Generic;

namespace ProyectoSenaLmour.Models;

public partial class Paquete
{
    public int IdPaquete { get; set; }

    public string? NomPaquete { get; set; }

    public double? Costo { get; set; }

    public int? IdHabitacion { get; set; }

    public string? Estado { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<DetalleReservaPaquete> DetalleReservaPaquetes { get; set; } = new List<DetalleReservaPaquete>();

    public virtual Habitacione? IdHabitacionNavigation { get; set; }

    public virtual ICollection<PaqueteServicio> PaqueteServicios { get; set; } = new List<PaqueteServicio>();
}
