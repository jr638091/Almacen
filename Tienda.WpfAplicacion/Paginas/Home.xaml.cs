using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tienda.Modelo;
using OfficeExcel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Drawing;

namespace WpfAplicacion
{
    /// <summary>
    /// Lógica de interacción para Home.xaml
    /// </summary>
    /// 

    
    public partial class Home : Page
    {
        public string Prueba { get { return "Hola"; } }

        public Home()
        {
            
            using (var db = new TiendaDbContext())
            {
                if(!db.Database.Exists())
                {
                    var find_db = new BuscarDBVentana();
                    find_db.ShowDialog();
                }
                if (db.Tiendas.Count() == 0)
                    db.SeedPublico(db);
            }
            
            InitializeComponent();
            
        }

        

        
    }

    
}
