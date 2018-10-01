using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Modelo;

namespace Tienda.Test
{
    public class TiendaDbContext : DbContext, Tienda_DataSource
    {
        public TiendaDbContext() : base("name=TiendaContext")
        {

        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Shop> Tiendas { get; set; }
        public DbSet<ReporteVenta> ReporteVentas { get; set; }
        public DbSet<ArticuloVenta> ArticuloVentas { get; set; }
        public DbSet<ReporteDeuda> ReporteDeudas { get; set; }
        public DbSet<ArticuloDeuda> ArticuloDeudas { get; set; }
        public DbSet<ReporteDevolucion> ReporteDevoluciones { get; set; }
        public DbSet<ArticuloDevolucion> ArticuloDevoluciones { get; set; }
        public DbSet<ReporteEntrada> ReporteEntradas { get; set; }
        public DbSet<ArticuloEntrada> ArticuloEntradas { get; set; }
        public DbSet<ReporteTransferencia> ReporteTransferencias { get; set; }
        public DbSet<ArticuloTransferencia> ArticuloTransferencias { get; set; }
        public DbSet<InformeLiquidacion> InformeLiquidaciones { get; set; }
        public DbSet<Trabajador> Trabajadores { get; set; }
        public DbSet<Existencia> Existencias { get; set; }
        public DbSet<InformePagoDeuda> PagoDeuda { get; set; }

        IQueryable<Producto> Tienda_DataSource.Productos => Productos;

        IQueryable<Shop> Tienda_DataSource.Tiendas => Tiendas;

        IQueryable<ReporteVenta> Tienda_DataSource.ReporteVentas => ReporteVentas;

        IQueryable<ArticuloVenta> Tienda_DataSource.ArticuloVentas => ArticuloVentas;

        IQueryable<ReporteDeuda> Tienda_DataSource.ReporteDeudas => ReporteDeudas;

        IQueryable<ArticuloDeuda> Tienda_DataSource.ArticuloDeudas => ArticuloDeudas;

        IQueryable<ReporteDevolucion> Tienda_DataSource.ReporteDevoluciones => ReporteDevoluciones;

        IQueryable<ArticuloDevolucion> Tienda_DataSource.ArticuloDevoluciones => ArticuloDevoluciones;

        IQueryable<ReporteEntrada> Tienda_DataSource.ReporteEntradas => ReporteEntradas;

        IQueryable<ArticuloEntrada> Tienda_DataSource.ArticuloEntradas => ArticuloEntradas;

        IQueryable<ReporteTransferencia> Tienda_DataSource.ReporteTransferencias => ReporteTransferencias;

        IQueryable<InformeLiquidacion> Tienda_DataSource.InformeLiquidaciones => InformeLiquidaciones;

        IQueryable<Trabajador> Tienda_DataSource.Trabajadores => Trabajadores;

        IQueryable<Existencia> Tienda_DataSource.Existencias => Existencias;

        IQueryable<ArticuloTransferencia> Tienda_DataSource.ArticuloTransferencias => ArticuloTransferencias;

        IQueryable<InformePagoDeuda> Tienda_DataSource.InformePagoDeuda => PagoDeuda;
    }
}
