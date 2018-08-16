using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Modelo
{
    public class Existencia
    {
        public virtual int ExistenciaId { get; set; }
        public virtual int ProductoId { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual int CantidadTotal { get; set; }
        public virtual int CantidadBuenEstado { get; set; }
        public virtual int CantidadDefectuoso { get; set; }
    }
}
