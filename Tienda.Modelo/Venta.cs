using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Modelo
{
    public class Venta
    {
        public virtual int VentaId { get; set; }
        public virtual int ProductoId { get; set; }
        public virtual ICollection<Tuple<Producto, int, double, bool>> Articulos { get; set; }
        public virtual int EmpleadoId { get; set; }
        public virtual Empleado Empleado { get; set; }
        public virtual DateTime Fecha { get; set; }
    }
}
