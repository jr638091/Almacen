using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Modelo;

namespace Tienda.Test
{
    public class Manager
    {
        private TiendaDbContext db;
        public Manager()
        {
            db = new TiendaDbContext(); 
        }

        
    }
}
