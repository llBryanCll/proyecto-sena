using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ProyectoSenaLmour.Models;

public partial class Reserva
{
    public int IdReserva { get; set; }

    [DisplayName("Cliente")]
    public int NroDocumentoCliente { get; set; }

    [DisplayName("Usuario")]
    public int NroDocumentoUsuario { get; set; }

    [DisplayName("Fecha de Reserva")]
    public DateTime FechaReserva { get; set; }

    [DisplayName("Fecha de Ingreso")]
    public DateTime FechaInicio { get; set; }

    [DisplayName("Fecha de Salida")]
    public DateTime FechaFinalizacion { get; set; }

    public double SubTotal { get; set; }

    public double Descuento { get; set; }

    public double Iva { get; set; }

    [DisplayName("Monto Total")]
    public double MontoTotal { get; set; }

    [DisplayName("Método de Pago")]
    public int IdMetodoPago { get; set; }

    [DisplayName("Número de Personas")]
    public int NroPersonas { get; set; }

    [DisplayName("Estado de reserva")]
    public int IdEstadoReserva { get; set; }

    public virtual ICollection<Abono> Abonos { get; set; } = new List<Abono>();

    public virtual ICollection<DetalleReservaPaquete> DetalleReservaPaquetes { get; set; } = new List<DetalleReservaPaquete>();

    public virtual ICollection<DetalleReservaServicio> DetalleReservaServicios { get; set; } = new List<DetalleReservaServicio>();

    [DisplayName("Estado de reserva")]
    public virtual EstadosReserva IdEstadoReservaNavigation { get; set; } = null!;

    [DisplayName("Método de Pago")]
    public virtual MetodoPago IdMetodoPagoNavigation { get; set; } = null!;

    [DisplayName("Cliente")]
    public virtual Cliente NroDocumentoClienteNavigation { get; set; } = null!;

    [DisplayName("Usuario")]
    public virtual Usuario NroDocumentoUsuarioNavigation { get; set; } = null!;
}
