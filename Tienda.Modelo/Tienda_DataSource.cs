using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Modelo
{
    public interface Tienda_DataSource
    {
        IQueryable<Almacen> Almacenes { get; }
        IQueryable<Articulo> Articulos { get; }
        IQueryable<Deuda> Deudas { get; }
        IQueryable<Empleado> Empleados { get; }
        IQueryable<Entrada> Entradas { get; }
        IQueryable<Existencia> Existencias { get; }
        IQueryable<Producto> Productos { get; }
        IQueryable<Shop> Tiendas { get; }
        IQueryable<Venta> Ventas { get; }
    }
}
