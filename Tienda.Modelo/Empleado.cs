using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Modelo
{
    public class Empleado
    {
        public virtual int EmpleadoId { get; set; }
        public virtual string Nombre { get; set; }
        public virtual int TiendaId { get; set; }
        public virtual Shop Tienda { get; set; }
    }
}
