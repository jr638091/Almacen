namespace WpfAplicacion.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using Tienda.Modelo;

    internal sealed class Configuration : DbMigrationsConfiguration<WpfAplicacion.TiendaDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WpfAplicacion.TiendaDbContext context)
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
    }
}
