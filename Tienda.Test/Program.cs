using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Modelo;

namespace Tienda.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new TiendaDbContext();
            var Tienda = new Shop { Nombre = "Tienda de Liuda" };
            db.Tiendas.Add(Tienda);
            db.SaveChanges();
            
        }


       
    }
}
