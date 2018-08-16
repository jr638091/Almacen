using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Modelo
{
    public class Shop
    {
        public virtual int ShopId { get; set; }
        public virtual string Nombre { get; set; }
        public virtual ICollection<Empleado> Personal { get; set; }
        public virtual ICollection<Articulo> Articulos { get; set; }
        public virtual ICollection<Almacen> Almacenes { get; set; }
    }
}
