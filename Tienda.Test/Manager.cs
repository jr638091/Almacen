using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Modelo;

namespace Tienda.Test
{
    //public class Manager
    //{
    //    private TiendaDbContext db;
    //    public Manager()
    //    {
    //        db = new TiendaDbContext(); 
    //    }

    //    public bool CrearProducto(string codigo, string descripcion)
    //    {
    //        if(db.Productos.Where(p => p.Codigo == codigo).Count() > 0)
    //        {
    //            return false;
    //        }
    //        var pro = new Producto { Codigo = codigo, Descripcion = descripcion };
    //        db.Productos.Add(pro);
    //        db.SaveChanges();

    //        return true;
    //    }

    //    public int CrearEmpleado(string Nombre, Shop tienda)
    //    {
    //        var empleado = new Empleado { Nombre = Nombre };
            
    //        empleado.Tienda = tienda;
            
    //        db.Empleados.Add(empleado);
    //        db.SaveChanges();
            
    //        return empleado.EmpleadoId;
    //    }

    //    public void CrearAlmacen(string Nombre)
    //    {
    //        var almacen = new Almacen { Nombre = Nombre };
    //        db.Almacenes.Add(almacen);
    //        db.SaveChanges();
    //    }

    //    public void EntradaAlmacen(Almacen almacen, Producto producto, int cantidad, bool estado)
    //    {
    //        if (estado)
    //        {
    //            if(almacen.Existencias.Where(p => p.Producto.Codigo == producto.Codigo).Count() > 0)
    //            {
    //                var exist = almacen.Existencias.Where(p => p.Producto.Codigo == producto.Codigo).First();
    //                exist.CantidadBuenEstado += cantidad;
    //                exist.CantidadTotal += cantidad;
    //            }
    //            else
    //            {
    //                var exist = new Existencia { Producto = producto, CantidadBuenEstado = cantidad, CantidadDefectuoso = 0, CantidadTotal = cantidad };
    //                db.Existencias.Add(exist);
    //                db.SaveChanges();
    //                almacen.Existencias.Add(exist);
    //            }
    //        }
    //        else
    //        {
    //            if (almacen.Existencias.Where(p => p.Producto.Codigo == producto.Codigo).Count() > 0)
    //            {
    //                var exist = almacen.Existencias.Where(p => p.Producto.Codigo == producto.Codigo).First();
    //                exist.CantidadDefectuoso += cantidad;
    //                exist.CantidadTotal += cantidad;
    //            }
    //            else
    //            {
    //                var exist = new Existencia { Producto = producto, CantidadDefectuoso = cantidad, CantidadBuenEstado = 0, CantidadTotal = cantidad };
    //                db.Existencias.Add(exist);
    //                db.SaveChanges();
    //                almacen.Existencias.Add(exist);
    //            }
    //        }
    //        db.Entry(almacen).State = EntityState.Modified;
    //        db.SaveChanges();
    //    }

    //    public Shop CrearTienda(string Nombre)
    //    {
    //        var tienda = new Shop { Nombre = Nombre };
    //        db.SaveChanges();
    //        return tienda;
    //    }
        
    //}
}
