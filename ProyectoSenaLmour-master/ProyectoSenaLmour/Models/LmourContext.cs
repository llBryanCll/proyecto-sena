using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoSenaLmour.Models;

public partial class LmourContext : DbContext
{
    public LmourContext()
    {
    }

    public LmourContext(DbContextOptions<LmourContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Abono> Abonos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DetalleReservaPaquete> DetalleReservaPaquetes { get; set; }

    public virtual DbSet<DetalleReservaServicio> DetalleReservaServicios { get; set; }

    public virtual DbSet<EstadosReserva> EstadosReservas { get; set; }

    public virtual DbSet<Habitacione> Habitaciones { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<Paquete> Paquetes { get; set; }

    public virtual DbSet<PaqueteServicio> PaqueteServicios { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<PermisosRole> PermisosRoles { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

    public virtual DbSet<TipoHabitacione> TipoHabitaciones { get; set; }

    public virtual DbSet<TipoServicio> TipoServicios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=Lmour;integrated security=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Abono>(entity =>
        {
            entity.HasKey(e => e.IdAbono).HasName("PK__Abono__A4693DA70BAD7444");

            entity.ToTable("Abono");

            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FechaAbono).HasColumnType("date");
            entity.Property(e => e.Iva).HasColumnName("IVA");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.Abonos)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("FK__Abono__IdReserva__693CA210");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.NroDocumento).HasName("PK__Clientes__CC62C91C195E96C5");

            entity.Property(e => e.NroDocumento).ValueGeneratedNever();
            entity.Property(e => e.Apellidos)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Celular)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Genero)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Nombres)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Clientes__IdRol__5441852A");

            entity.HasOne(d => d.IdTipoDocumentoNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdTipoDocumento)
                .HasConstraintName("FK__Clientes__IdTipo__5535A963");
        });

        modelBuilder.Entity<DetalleReservaPaquete>(entity =>
        {
            entity.HasKey(e => e.DetalleReservaPaquete1).HasName("PK__DetalleR__2E8BFF25F70131E2");

            entity.ToTable("DetalleReservaPaquete");

            entity.Property(e => e.DetalleReservaPaquete1).HasColumnName("DetalleReservaPaquete");

            entity.HasOne(d => d.IdPaqueteNavigation).WithMany(p => p.DetalleReservaPaquetes)
                .HasForeignKey(d => d.IdPaquete)
                .HasConstraintName("FK__DetalleRe__IdPaq__656C112C");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.DetalleReservaPaquetes)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("FK__DetalleRe__IdRes__66603565");
        });

        modelBuilder.Entity<DetalleReservaServicio>(entity =>
        {
            entity.HasKey(e => e.IdDetalleReservaServicio).HasName("PK__DetalleR__D3D91A5AF3892295");

            entity.ToTable("DetalleReservaServicio");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.DetalleReservaServicios)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("FK__DetalleRe__IdRes__628FA481");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.DetalleReservaServicios)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK__DetalleRe__IdSer__619B8048");
        });

        modelBuilder.Entity<EstadosReserva>(entity =>
        {
            entity.HasKey(e => e.IdEstadoReserva).HasName("PK__EstadosR__3E654CA83F7D07AF");

            entity.ToTable("EstadosReserva");

            entity.Property(e => e.NombreEstadoReserva)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Habitacione>(entity =>
        {
            entity.HasKey(e => e.IdHabitacion).HasName("PK__Habitaci__8BBBF901CD0EC5B7");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTipoHabitacionNavigation).WithMany(p => p.Habitaciones)
                .HasForeignKey(d => d.IdTipoHabitacion)
                .HasConstraintName("FK__Habitacio__IdTip__440B1D61");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.IdMetodoPago).HasName("PK__MetodoPa__6F49A9BEE902835A");

            entity.ToTable("MetodoPago");

            entity.Property(e => e.NomMetodoPago)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Paquete>(entity =>
        {
            entity.HasKey(e => e.IdPaquete).HasName("PK__Paquetes__DE278F8B0AE9AE2F");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.NomPaquete)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.Paquetes)
                .HasForeignKey(d => d.IdHabitacion)
                .HasConstraintName("FK__Paquetes__IdHabi__4BAC3F29");
        });

        modelBuilder.Entity<PaqueteServicio>(entity =>
        {
            entity.HasKey(e => e.IdPaqueteServicio).HasName("PK__PaqueteS__DE5C2BC2D8A2C198");

            entity.ToTable("PaqueteServicio");

            entity.HasOne(d => d.IdPaqueteNavigation).WithMany(p => p.PaqueteServicios)
                .HasForeignKey(d => d.IdPaquete)
                .HasConstraintName("FK__PaqueteSe__IdPaq__4E88ABD4");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.PaqueteServicios)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK__PaqueteSe__IdSer__4F7CD00D");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PK__Permisos__0D626EC827AA48B3");

            entity.Property(e => e.NomPermiso)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PermisosRole>(entity =>
        {
            entity.HasKey(e => e.IdPermisosRoles).HasName("PK__Permisos__8C257ED919DFBB31");

            entity.HasOne(d => d.IdPermisoNavigation).WithMany(p => p.PermisosRoles)
                .HasForeignKey(d => d.IdPermiso)
                .HasConstraintName("FK__PermisosR__IdPer__3C69FB99");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.PermisosRoles)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__PermisosR__IdRol__3B75D760");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("PK__Reservas__0E49C69D110C242B");

            entity.Property(e => e.FechaFinalizacion).HasColumnType("date");
            entity.Property(e => e.FechaInicio).HasColumnType("date");
            entity.Property(e => e.FechaReserva).HasColumnType("date");
            entity.Property(e => e.Iva).HasColumnName("IVA");

            entity.HasOne(d => d.IdEstadoReservaNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdEstadoReserva)
                .HasConstraintName("FK__Reservas__IdEsta__5DCAEF64");

            entity.HasOne(d => d.IdMetodoPagoNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdMetodoPago)
                .HasConstraintName("FK__Reservas__IdMeto__5EBF139D");

            entity.HasOne(d => d.NroDocumentoClienteNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.NroDocumentoCliente)
                .HasConstraintName("FK__Reservas__NroDoc__5BE2A6F2");

            entity.HasOne(d => d.NroDocumentoUsuarioNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.NroDocumentoUsuario)
                .HasConstraintName("FK__Reservas__NroDoc__5CD6CB2B");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__2A49584CDD96E9F0");

            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.NomRol)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio).HasName("PK__Servicio__2DCCF9A2D6E0532C");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.NomServicio)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTipoServicioNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdTipoServicio)
                .HasConstraintName("FK__Servicios__IdTip__48CFD27E");
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipoDocumento).HasName("PK__TipoDocu__3AB3332F27E0DE2C");

            entity.ToTable("TipoDocumento");

            entity.Property(e => e.NomTipoDcumento)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoHabitacione>(entity =>
        {
            entity.HasKey(e => e.IdTipoHabitacion).HasName("PK__TipoHabi__AB75E87C2680033D");

            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.NomTipoHabitacion)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoServicio>(entity =>
        {
            entity.HasKey(e => e.IdTipoServicio).HasName("PK__TipoServ__E29B3EA75FB1135D");

            entity.Property(e => e.NombreTipoServicio)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.NroDocumento).HasName("PK__Usuarios__CC62C91C1030C7D2");

            entity.Property(e => e.NroDocumento).ValueGeneratedNever();
            entity.Property(e => e.Apellidos)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Celular)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Genero)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Nombres)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuarios__IdRol__3F466844");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
