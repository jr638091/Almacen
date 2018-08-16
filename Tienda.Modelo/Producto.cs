using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Modelo
{
    public class Producto
    {
        public virtual int ProductoId { get; set; }
        public virtual string Codigo { get; set; }
        public virtual string Descripcion { get; set; }

    }
}
