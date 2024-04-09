using System;
using System.Collections.Generic;

namespace ProyectoSenaLmour.Models;

public partial class Servicio
{
    public int IdServicio { get; set; }

    public int? IdTipoServicio { get; set; }

    public string? NomServicio { get; set; }

    public double? Costo { get; set; }

    public string? Descripcion { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<DetalleReservaServicio> DetalleReservaServicios { get; set; } = new List<DetalleReservaServicio>();

    public virtual TipoServicio? IdTipoServicioNavigation { get; set; }

    public virtual ICollection<PaqueteServicio> PaqueteServicios { get; set; } = new List<PaqueteServicio>();
}
