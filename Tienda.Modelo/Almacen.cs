using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Modelo
{
    public class Almacen
    {
        public virtual int AlmacenId { get; set; }
        public virtual string Nombre { get; set; }
        public virtual ICollection<Existencia> Existencias { get; set; }
    }
}
