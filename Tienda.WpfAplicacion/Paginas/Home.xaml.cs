using System;
using System.Collections.Generic;
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

namespace WpfAplicacion
{
    /// <summary>
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
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

        private void tbock_reportes_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                var reportes = new List<string>();
                var rep = db.ReporteEntradas.ToList();
                foreach (var reporte in rep)
                    reportes.Add(reporte.EscribirReporte());
                reportes.Sort();
                foreach (var reporte in reportes)
                    tbock_reportes.Text += reporte;
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var page = new MiTiendaPagina();
            this.NavigationService.Navigate(page);
        }
    }
}
