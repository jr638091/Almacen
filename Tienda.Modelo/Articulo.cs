using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Modelo
{
    public class Articulo
    {
        public virtual int ArticuloId { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual int CantidadTotal { get; set; }
        public virtual int CantidadBuenEstado { get; set; }
        public virtual int CantidadDefectuoso { get; set; }
        public virtual Tuple<int, DateTime> PrecioBuenEstado { get; set; }
        public virtual Tuple<int, DateTime> PrecioDefectuoso { get; set; }
        public virtual ICollection<Tuple<int, DateTime>> PrecioBuenEstadoHistorial { get; set; }
        public virtual ICollection<Tuple<int, DateTime>> PrecioDefectuosoHistorial { get; set; }

    }
}
