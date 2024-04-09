using System;
using System.Collections.Generic;

namespace ProyectoSenaLmour.Models;

public partial class TipoHabitacione
{
    public int IdTipoHabitacion { get; set; }

    public string? NomTipoHabitacion { get; set; }

    public int? NumeroPersonas { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<Habitacione> Habitaciones { get; set; } = new List<Habitacione>();
}
