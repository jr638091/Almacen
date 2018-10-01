using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Modelo;

namespace WpfAplicacion
{
    public class TiendaDbContext : DbContext, Tienda_DataSource
    {
        public TiendaDbContext() : base("name=TiendaContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public void SeedPublico(TiendaDbContext context)
        {
            var liuda = new Trabajador { Nombre = "Liuda" };
            var betty = new Trabajador { Nombre = "Betty" };

            //var abrigo1 = new Producto { Codigo = "AB1", Descripcion = "Abrigo de piel azul" };
            //var abrigo2 = new Producto { Codigo = "AB2", Descripcion = "Abrigo azul" };
            //var pullover1 = new Producto { Codigo = "P1", Descripcion = "Rojo y blaco es supreme" };
            //var pullover2 = new Producto { Codigo = "P2", Descripcion = "Rojo y blanco pero no supreme" };

            //var exist1 = new Existencia
            //{
            //    Producto = abrigo1,
            //    CantidadBuenEstado = 2,
            //    CantidadDefectuoso = 3,
            //    PrecioBuenEstado = 14,
            //    PrecioDefectuoso = 6
            //};

            //var exist2 = new Existencia
            //{
            //    Producto = abrigo2,
            //    CantidadBuenEstado = 2,
            //    CantidadDefectuoso = 3,
            //    PrecioBuenEstado = 12,
            //    PrecioDefectuoso = 10
            //};

            //var exist3 = new Existencia
            //{
            //    Producto = pullover1,
            //    CantidadBuenEstado = 2,
            //    CantidadDefectuoso = 3,
            //    PrecioBuenEstado = 20,
            //    PrecioDefectuoso = 5
            //};

            //var exist4 = new Existencia
            //{
            //    Producto = pullover2,
            //    CantidadBuenEstado = 2,
            //    CantidadDefectuoso = 3,
            //    PrecioBuenEstado = 10,
            //    PrecioDefectuoso = 4
            //};

            //var existencias = new List<Existencia> { exist1, exist2, exist3, exist4 };

            var tienda_principal = new Shop
            {
                Direccion = "Agustina",
                Nombre = "Tienda de Liuda",
                Telefono = 12345678,
                Encargado = liuda,
                Trabajadores = new List<Trabajador> { betty, liuda },
                Productos = new List<Existencia>(),
            };

            //var exist5 = new Existencia
            //{
            //    Producto = pullover1,
            //    CantidadBuenEstado = 2,
            //    CantidadDefectuoso = 3,
            //    PrecioBuenEstado = 20,
            //    PrecioDefectuoso = 5
            //};

            //var exist6 = new Existencia
            //{
            //    Producto = abrigo1,
            //    CantidadBuenEstado = 2,
            //    CantidadDefectuoso = 3,
            //    PrecioBuenEstado = 14,
            //    PrecioDefectuoso = 6
            //};

            //var exist7 = new Existencia
            //{
            //    Producto = abrigo2,
            //    CantidadBuenEstado = 2,
            //    CantidadDefectuoso = 3,
            //    PrecioBuenEstado = 12,
            //    PrecioDefectuoso = 10
            //};

            //var exist8 = new Existencia
            //{
            //    Producto = pullover2,
            //    CantidadBuenEstado = 2,
            //    CantidadDefectuoso = 3,
            //    PrecioBuenEstado = 10,
            //    PrecioDefectuoso = 4
            //};
            //var pepe = new Trabajador { Nombre = "Cuquita" };

            //var tienda_pepe = new Shop
            //{
            //    Direccion = "Sancti Spiritus",
            //    Nombre = "Tienda de Cuquita",
            //    Telefono = 11223344,
            //    Encargado = pepe,
            //    Trabajadores = new List<Trabajador> { },
            //    Productos = new List<Existencia> { },
            //};


            if (context.Tiendas.Count() == 0)
            {
                //context.Productos.Add(abrigo1);
                //context.Productos.Add(abrigo2);
                //context.Productos.Add(pullover1);
                //context.Productos.Add(pullover2);
                context.Trabajadores.Add(liuda);
                context.Trabajadores.Add(betty);
                context.SaveChanges();
                //existencias.ForEach(p => context.Existencias.Add(p));
                context.Tiendas.Add(tienda_principal);
                context.SaveChanges();
                //context.Tiendas.Add(tienda_pepe);
                context.SaveChanges();

            }
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
        public DbSet<InformePagoDeuda> InformePagoDeudas { get; set; }

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

        IQueryable<InformePagoDeuda> Tienda_DataSource.InformePagoDeuda => InformePagoDeudas;
    }
}
