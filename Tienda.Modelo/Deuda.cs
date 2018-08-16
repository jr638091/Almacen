using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Modelo
{
    public class Deuda
    {
        public virtual int DeudaId { get; set; }
        public virtual ICollection<Tuple<Producto, int, double, bool>> Articulos { get; set; }
        public virtual double Pagado { get; set; }
        public virtual int TiendaId { get; set; }
        public virtual Shop Tienda { get; set; }
        public virtual DateTime Fecha { get; set; }
    }
}
