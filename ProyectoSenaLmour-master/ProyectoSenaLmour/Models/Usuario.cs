using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace ProyectoSenaLmour.Models;

public partial class Usuario
{
    public int NroDocumento { get; set; }

    public int IdTipoDocumento { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Celular { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Genero { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public int? IdRol { get; set; }

    //por aca
    public string ConfirmarClave { get; set; }

    public bool Restablecer { get; set; }

    public bool Confirmado { get; set; }
    public string Token { get; set; }
    //aca

    public virtual Role? IdRolNavigation { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
