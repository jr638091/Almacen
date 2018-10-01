using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Modelo;
using System.Configuration;

namespace Tienda.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new TiendaDbContext())
            {
                Console.WriteLine(db.Productos.First().Codigo);
            }
            string principio = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=";
            string final = ";Integrated Security=True;Connect Timeout=30";
            string medio = Console.ReadLine();


            string conexion = principio + medio + final;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings["TiendaContext"].ConnectionString = conexion;
            
            config.Save();
            using (var db = new TiendaDbContext())
            {
                Console.WriteLine( db.Productos.First().Codigo);
            }
        }

        


    }
}
