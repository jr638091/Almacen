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

        public DbSet<Almacen> Almacenes { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Deuda> Deudas { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Existencia> Existencias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Shop> Tiendas { get; set; }
        public DbSet<Venta> Ventas { get; set; }

        IQueryable<Almacen> Tienda_DataSource.Almacenes => Almacenes;

        IQueryable<Articulo> Tienda_DataSource.Articulos => Articulos;

        IQueryable<Deuda> Tienda_DataSource.Deudas => Deudas;

        IQueryable<Empleado> Tienda_DataSource.Empleados => Empleados;

        IQueryable<Entrada> Tienda_DataSource.Entradas => Entradas;

        IQueryable<Existencia> Tienda_DataSource.Existencias => Existencias;

        IQueryable<Producto> Tienda_DataSource.Productos => Productos;

        IQueryable<Shop> Tienda_DataSource.Tiendas => Tiendas;

        IQueryable<Venta> Tienda_DataSource.Ventas => Ventas;
    }
}
