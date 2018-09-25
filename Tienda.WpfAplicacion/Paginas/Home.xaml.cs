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
        public Home()
        {
            using (var db = new TiendaDbContext())
            {
                db.Productos.First();
            }
            
            
            InitializeComponent();
            this.DataContext = this;

        }

        private void Producto_Button_Click(object sender, RoutedEventArgs e)
        {
            var p = new Productos();
            this.NavigationService.Navigate(p);
        }

        private void Mis_Tiendas_Click(object sender, RoutedEventArgs e)
        {
            var p = new MisTiendas();
            this.NavigationService.Navigate(p);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var page = new MiTiendaPagina();
            this.NavigationService.Navigate(page);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Paginas.Estadisticas());
        }
    }

    
}
