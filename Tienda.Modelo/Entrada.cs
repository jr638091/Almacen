using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Modelo
{
    public class Entrada
    {
        public virtual int EntradaId { get; set; }
        public virtual int ProductoId { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual int CantidadBuenEstado { get; set; }
        public virtual int CantidadDefectuoso { get; set; }
        public virtual int AlmacenId { get; set; }
        public virtual Almacen Almacen { get; set; }
        public virtual DateTime FechaEntrada { get; set; }

    }
}
