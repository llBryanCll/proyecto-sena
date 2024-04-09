using System;
using System.Collections.Generic;

namespace ProyectoSenaLmour.Models;

public partial class Cliente
{
    public int NroDocumento { get; set; }

    public int? IdTipoDocumento { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string? Celular { get; set; }

    public string? Correo { get; set; }

    public string? Contraseña { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? Genero { get; set; }

    public string? Estado { get; set; }

    public int? IdRol { get; set; }

    public virtual Role? IdRolNavigation { get; set; }

    public virtual TipoDocumento? IdTipoDocumentoNavigation { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
